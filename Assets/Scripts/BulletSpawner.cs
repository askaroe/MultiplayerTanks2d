using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; 

public class BulletSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private Transform _initialTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && IsOwner)
        {
            SpawnBulletServerRpc(_initialTransform.position, _initialTransform.rotation);
        }
    }

    [ServerRpc]
    private void SpawnBulletServerRpc(Vector2 position, Quaternion rotation)
    {
        var instantiatedBullet = Instantiate(_bullet, position, rotation);
        instantiatedBullet.GetComponent<NetworkObject>().Spawn();
    }
    

    
}