using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
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

    private void Awake()
    {
        _instance = this;
    }

    [SerializeField]
    private GameObject _popUp;
    [SerializeField]
    private Text _playerWinText;

    public void ShowPopUpWin(string playerName)
    {
        _popUp.SetActive(true);
        _playerWinText.text = playerName + " wins!";
    }

    public void ShowPopLose()
    {
        _popUp.SetActive(true);
    }

    public void ToMainLobby()
    {
        GameManager.Instance.LeaveGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
