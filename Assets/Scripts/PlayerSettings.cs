using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Sprite[] _playerSprites;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _spriteRenderer.sprite = _playerSprites[(int)OwnerClientId];
        switch (OwnerClientId)
        {
            case 0:
                transform.position = new Vector3(-5.45f, -3.34f, 0);
                break;
            case 1:
                transform.position = new Vector3(5.0f, 3.4f, 0);
                transform.rotation = Quaternion.Euler(0, 0, -180f);
                break;
            case 2:
                transform.position = new Vector3(5.0f, -3.3f, 0);
                break;
            case 3:
                transform.position = new Vector3(-5.4f, 3.4f, 0);
                transform.rotation = Quaternion.Euler(0, 0, -180f);
                break;
            default:
                Debug.Log("Lobby full");
                break;
        }
    }
}
