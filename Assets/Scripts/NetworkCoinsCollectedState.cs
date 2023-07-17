using Unity.Netcode;
using UnityEngine;

public class NetworkCoinsCollectedState : NetworkBehaviour
{
    [HideInInspector] public NetworkVariable<int> coinsCollected = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        coinsCollected.Value = 0;

    }
}
