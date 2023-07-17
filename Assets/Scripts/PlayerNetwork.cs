using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]
    private Transform _bulletSpawnPoint;
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private float _playerSpeed = 2.0f;
    [SerializeField] 
    private GameObject _playerUI;

    private bool _isTurnedOn = false;

    private NetworkVariable<int> _playerLife = new NetworkVariable<int>(3,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private BoxCollider2D _collider;
    private bool _isDestroyed = false;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (SceneManager.GetActiveScene().name == "SampleScene" && !_isTurnedOn)
        {
            _playerUI.SetActive(true);
        }

        Movement();

    }

    void Movement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if(horizontalInput != 0 && !_isDestroyed)
        {
            transform.position += new Vector3(horizontalInput * _playerSpeed * Time.deltaTime, 0f, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, -90.0f * horizontalInput);
        }
        else if(verticalInput != 0 && !_isDestroyed)
        {
            transform.position += new Vector3(0, verticalInput * _playerSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, 90.0f - 90.0f * verticalInput);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8.4f, 8.4f),
            Mathf.Clamp(transform.position.y, -4.5f, 4.5f), 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;
        if (other.GetComponent<Bullet>())
        {
            GetComponent<NetworkHealthState>().healthPoint.Value -= 1;
        }
    }

    public void PlayerDestroyed()
    {
        _playerSpeed = 0f;
        _collider.enabled = false;
        _isDestroyed = true;
    }

}
