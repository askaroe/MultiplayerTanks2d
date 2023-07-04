using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _lobbyButtons;
    [SerializeField]
    private GameObject _lobbyCreationState;
    [SerializeField]
    private GameObject _lobbyJoinState;
    private GameObject _currentState;

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


}
