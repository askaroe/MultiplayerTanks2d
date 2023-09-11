using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Singleton

    private static PlayerManager _instance;

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Your instance is null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    
    //
    
    private int _destroyedPlayers;
    private List<string> _playersNames = new List<string> { "Blue", "Red", "Green", "Yellow" };
    [SerializeField]
    private List<int> _playersIDs;

    //public void UpdateDestroyedPlayersCount()
    //{
    //    _destroyedPlayers += 1;
    //    if (_destroyedPlayers == GameManager.Instance.maxPlayers - 1)
    //    {
    //        UIManager.Instance.ShowPopUp();
    //    }
    //}

    private void Start()
    {
        _playersIDs = new List<int>();
    }

    public void AddPlayersToList(int playerID)
    {
        _playersIDs.Add(playerID);
    }

    public void DeletePLayerFromList(int playerID)
    {
        Debug.Log("Deleting player...");
        Debug.Log(_playersIDs.Count);
        for (int i = 0; i < _playersIDs.Count; i++)
        {
            Debug.Log("In loop...");
            if (_playersIDs[i] == playerID)
            {
                _playersIDs.RemoveAt(i);
                Debug.Log("Removed! " + _playersIDs.Count.ToString());
            }
        }

        Debug.Log(_playersIDs.Count);

        if (_playersIDs.Count == 1)
        {
            UIManager.Instance.ShowPopUpWin(_playersNames[_playersIDs[0]]);
            Debug.Log(_playersNames[_playersIDs[0]]);
        }

        else if(_playersIDs.Count == 0)
        {
            UIManager.Instance.ShowPopLose();
        }
    }
}
