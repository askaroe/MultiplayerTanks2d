using UnityEngine;

public class Dropdown : MonoBehaviour
{
    public void PlayerCount(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.Instance.maxPlayers = 2;
                break;
            case 1:
                GameManager.Instance.maxPlayers = 3;
                break;
            case 2:
                GameManager.Instance.maxPlayers = 4;
                break;
        }
    }
}
