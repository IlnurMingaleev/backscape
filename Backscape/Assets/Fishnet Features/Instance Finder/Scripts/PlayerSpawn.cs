using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;
using FishNet.Connection;


public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public override void OnStartClient()
    {
        base.OnStartClient();
        ServerSpawnRequest();
        

    }
    
    private void ServerSpawnRequest()
    {
        GameObject result = Instantiate(playerPrefab);

        base.Spawn(result, base.Owner);
    }
}
