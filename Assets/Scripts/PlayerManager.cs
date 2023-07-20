using System;
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
    
    public void UpdateDestroyedPlayersCount()
    {
        _destroyedPlayers += 1;
        if (_destroyedPlayers == GameManager.Instance.maxPlayers - 1)
        {
            UIManager.Instance.ShowPopUp();
        }
    }
}
