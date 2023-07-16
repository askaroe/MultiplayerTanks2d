using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
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
    private GameObject _popUpWin;
    [SerializeField]
    private GameObject _popUpLose;
    [SerializeField]
    private GameObject _restartButton;
    [SerializeField]
    private Text _playerWinText;
    [SerializeField]
    private Text _coinsEarnedTextLose;
    [SerializeField]
    private Text _coinsEarnedTextWin;
    [SerializeField]
    private Text _healthPointsText;


    private Dictionary<int, string> _dict = new Dictionary<int, string>
    {
        {0, "Blue " },
        {1, "Red " },
        {2, "Green " },
        {3, "Yellow " }
    };

    public void ShowPopUpLose(int coins)
    {
        _popUpLose.SetActive(true);
        _coinsEarnedTextLose.text = "Coins Collected: " + coins.ToString();
    }

    public void ShowPopUpWin(int playerId, int coins)
    {
        _popUpWin.SetActive(true);
        _playerWinText.text = _dict[playerId] + "Wins!";
        _coinsEarnedTextWin.text = "Coins Collected: " + coins.ToString();
    }


}
