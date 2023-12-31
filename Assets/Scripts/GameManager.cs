using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager _instance;
    public static GameManager Instance => _instance;

    private string _lobbyId;
    public string lobbyName;
    public string joinLobbyByName;
    public int maxPlayers = 2;
    [SerializeField]
    private GameObject _playerPrefab;

    private RelayHostData _hostData;
    private RelayJoinData _joinData;

    // Setup events

    // Notify state update
    public UnityAction<string> UpdateState;
    // Notify Match found
    public UnityAction MatchFound;


    private void Awake()
    {
        // Just a basic singleton
        if (_instance is null)
        {
            _instance = this;
            return;
        }

        Destroy(this);
    }

    async void Start()
    {
        // Initialize unity services
        await UnityServices.InitializeAsync();

        // Setup events listeners
        SetupEvents();

        // Unity Login
        await SignInAnonymouslyAsync();

        // Subscribe to NetworkManager events
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
    }


    #region Network events

    private void ClientConnected(ulong id)
    {
        // Player with id connected to our session

        Debug.Log("Connected player with id: " + id);
        UpdateState?.Invoke("Player found!");
        MatchFound?.Invoke();
        LobbyUI.Instance.UpdatePlayersCountInLobbyCreate((int)(id + 1));
        LobbyUI.Instance.UpdatePlayersCountInLobbyJoin((int)(id + 1));
        if((int)(id+1) == maxPlayers)
        {
            LobbyUI.Instance.StartGameButton();
        }
    }

    #endregion

    #region UnityLogin

    void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");
        }
        catch (Exception ex)
        {
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    #endregion

    #region Lobby

    public async void FindMatch()
    {
        Debug.Log("Looking for a lobby...");

        UpdateState?.Invoke("Looking for a match...");

        try
        {
            // Looking for a lobby

            //Add options to the matchmaking(mode, rank, etc..)
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();

            // Quick-join a random lobby
            var lobby = await Lobbies.Instance.QuickJoinLobbyAsync(options);

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            foreach (var lobbyitr in queryResponse.Results)
            {
                if (lobbyitr.Name == lobbyName && lobbyitr.Name == joinLobbyByName)
                {
                    lobby = lobbyitr;
                    Debug.Log("found!!!");
                }
            }

            Debug.Log("Joined lobby: " + lobby.Id);
            Debug.Log("Lobby Players: " + lobby.Players.Count);

            // Retrieve the Relay code previously set in the create match
            string joinCode = lobby.Data["joinCode"].Value;

            Debug.Log("Received code: " + joinCode);

            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            // Create Object
            _joinData = new RelayJoinData
            {
                Key = allocation.Key,
                Port = (ushort)allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                HostConnectionData = allocation.HostConnectionData,
                IPv4Address = allocation.RelayServer.IpV4
            };

            // Set transport data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                _joinData.IPv4Address,
                _joinData.Port,
                _joinData.AllocationIDBytes,
                _joinData.Key,
                _joinData.ConnectionData,
                _joinData.HostConnectionData);

            // Finally start the client
            NetworkManager.Singleton.StartClient();

            // Trigger events
            UpdateState?.Invoke("Match found! LObby name: " + lobby.Name);
            MatchFound?.Invoke();
        }
        catch (LobbyServiceException e)
        {
            // If we don't find any lobby, let's create a new one
            Debug.Log("Cannot find a lobby: " + e);
            
        }
    }

    public async void CreateMatch()
    {
        Debug.Log("Creating a new lobby...");

        LobbyUI.Instance.HideButtonAfterLobbyCreated();

        UpdateState?.Invoke("Creating a new match...");

        // External connections
        int maxConnections = maxPlayers - 1;

        try
        {
            // Create RELAY object
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            _hostData = new RelayHostData
            {
                Key = allocation.Key,
                Port = (ushort)allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                IPv4Address = allocation.RelayServer.IpV4
            };

            // Retrieve JoinCode
            _hostData.JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            if (lobbyName.Length == 0) lobbyName = "game_lobby";
            
            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = false;

            // Put the JoinCode in the lobby data, visible by every member
            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "joinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: _hostData.JoinCode)
                },
            };

            // Create the lobby
            var lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            // Save Lobby ID for later uses
            _lobbyId = lobby.Id;


            Debug.Log("Created lobby: " + lobby.Id + " " + lobby.Name);

            // Heartbeat the lobby every 15 seconds.
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

            // Now that RELAY and LOBBY are set...


            // Set Transports data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                _hostData.IPv4Address,
                _hostData.Port,
                _hostData.AllocationIDBytes,
                _hostData.Key,
                _hostData.ConnectionData
                );

            

            // Finally start host
            NetworkManager.Singleton.StartHost();
            //NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            //NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);


            UpdateState?.Invoke("Waiting for players...");

            
        }
        catch (LobbyServiceException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            Debug.Log("Lobby Heartbit");
            yield return delay;
        }
    }

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        _playerPrefab.SetActive(true);
    }

    public void LeaveGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        _playerPrefab.SetActive(false);
    }

    private void OnDestroy()
    {
        // We need to delete the lobby when we're not using it
        Lobbies.Instance.DeleteLobbyAsync(_lobbyId);
    }

    #endregion

    /// <summary>
    /// RelayHostData represents the necessary informations
    /// for a Host to host a game on a Relay
    /// </summary>
    public struct RelayHostData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] Key;
    }

    /// <summary>
    /// RelayHostData represents the necessary informations
    /// for a Host to host a game on a Relay
    /// </summary>
    public struct RelayJoinData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] HostConnectionData;
        public byte[] Key;
    }
}