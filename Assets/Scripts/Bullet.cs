using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class Bullet : NetworkBehaviour
{
    [SerializeField]
    private float _bulletSpeed = 10.0f;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody2D>().velocity = this.transform.up * _bulletSpeed;
        if (IsHost)
        {
            DespawningServerRpc();
        }

        else if (IsClient)
        {
            DespawningClientRpc();
        }


    }

    IEnumerator BulletDespawning()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<NetworkObject>().Despawn(true);
    }

    [ServerRpc]
    private void DespawningServerRpc()
    {
        StartCoroutine(BulletDespawning());
    }

    [ClientRpc]
    private void DespawningClientRpc()
    {
        StartCoroutine(BulletDespawning());
    }

    [ServerRpc]
    public void DespawnServerRpc()
    {
        GetComponent<NetworkObject>().Despawn(true);
    }

    [ClientRpc]
    public void DespawnClientRpc()
    {
        GetComponent<NetworkObject>().Despawn(true);
    }
}
