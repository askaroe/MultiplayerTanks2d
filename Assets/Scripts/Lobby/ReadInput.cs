using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInput : MonoBehaviour
{
    private string _input;
    
    public void CreateLobbyName(string s)
    {
        _input = s;
        GameManager.Instance.lobbyName = s;
        Debug.Log(_input);
    }

    public void JoinLobbyByName(string s)
    {
        GameManager.Instance.joinLobbyByName = s;
    }
}
