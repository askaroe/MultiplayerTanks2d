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

    private Dictionary<int, string> _dict = new Dictionary<int, string>
    {
        { 0, "Blue " },
        { 1, "Red " },
        { 2, "Green " },
        { 3, "Yellow " }
    };

    public void ShowPopUp()
    {
        _popUp.SetActive(true);
        _playerWinText.text = "Game Over!";
    }


}
