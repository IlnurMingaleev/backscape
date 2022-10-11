using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;
using FishNet.Connection;


public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public override void OnStartClient()
    {
        base.OnStartClient();
        ServerSpawnRequest();
        
        

    }
    [ServerRpc]
    private void ServerSpawnRequest()
    {
        GameObject result = Instantiate(playerPrefab);

        base.Spawn(result, base.Owner);
    }
}
