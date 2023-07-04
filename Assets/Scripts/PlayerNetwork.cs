using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]
    private Transform _bulletSpawnPoint;
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private float _playerSpeed = 2.0f;


    private void Update()
    {
        if (!IsOwner) return;

        Movement();
        
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if(horizontalInput != 0)
        {
            transform.position += new Vector3(horizontalInput * _playerSpeed * Time.deltaTime, 0f, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, -90.0f * horizontalInput);
        }
        else if(verticalInput != 0)
        {
            transform.position += new Vector3(0, verticalInput * _playerSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, 90.0f - 90.0f * verticalInput);
        }
    }

}
