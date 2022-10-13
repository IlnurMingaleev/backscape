using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Observing;

public class CubeColorNetwork : NetworkBehaviour
{
    private Color endColor;
    private NetworkObject networkObject;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            endColor = SingletonColors.GetRandomColor();
            ChangeColorServer(gameObject,endColor);

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeColorServer(GameObject gameObj, Color color)
    {
        ChangeColor(gameObj, color);
    }
    [ObserversRpc(IncludeOwner = false)]
    public void ChangeColor(GameObject gameObj, Color color)
    {
        gameObj.GetComponent<MeshRenderer>().material.color = color;
    }
}
