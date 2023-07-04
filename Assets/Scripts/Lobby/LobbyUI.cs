using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    private static LobbyUI _instance;

    public static LobbyUI Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Intance is NULL!");
            }
            return _instance;
        }
    }
    
    
    [SerializeField]
    private GameObject _lobbyButtons;
    [SerializeField]
    private GameObject _lobbyCreationState;
    [SerializeField]
    private GameObject _lobbyJoinState;
    private GameObject _currentState;
    [SerializeField]
    private Text _playersInLobbyCreateText;
    [SerializeField]
    private Text _playersInLobbyJoinText;
    [SerializeField]
    private GameObject _startGameButton;


    private void Awake()
    {
        _instance = this;
    }

    public void CreateLobbyButton()
    {
        _lobbyButtons.SetActive(false);
        _lobbyCreationState.SetActive(true);
        _currentState = _lobbyCreationState;
    }

    public void JoinLobbyButton()
    {
        _lobbyButtons.SetActive(false);
        _lobbyJoinState.SetActive(true);
        _currentState = _lobbyJoinState;
    }

    public void BackButton()
    {
        _currentState.SetActive(false);
        _lobbyButtons.SetActive(true);
    }

    public void UpdatePlayersCountInLobbyCreate(int playerCount_text)
    {
        
        _playersInLobbyCreateText.text = "PLayers in Lobby: " + playerCount_text.ToString();
    }

    public void UpdatePlayersCountInLobbyJoin(int playerCount_text)
    {
        _playersInLobbyJoinText.text = "PLayers in Lobby: " + playerCount_text.ToString();
    }

    public void StartGameButton()
    {
        _startGameButton.SetActive(true);
    }

}
