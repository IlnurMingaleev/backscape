using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Observing;

public class PlayerColorNetwork : NetworkBehaviour
{
    [SerializeField] private GameObject playerBody;
    [SerializeField] private Color endColor;
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        //networkObject = GetComponent<NetworkObject>();
        //networkObject.GiveOwnership(base.Owner);
        if(base.IsOwner)
        {


        }
        else
        {
            GetComponent<PlayerColorNetwork>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            ChangeColorServer(gameObject,endColor);

        }
    }

    [ServerRpc]
    public void ChangeColorServer(GameObject gameObj, Color color)
    {
        ChangeColor(gameObj, color);
    }
    [ObserversRpc(IncludeOwner = false)]
    public void ChangeColor(GameObject gameObj, Color color)
    {
        gameObj.GetComponent<PlayerColorNetwork>()
        .playerBody.GetComponent<Renderer>().material.color = color;
    }
}

