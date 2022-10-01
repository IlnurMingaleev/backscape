using UnityEngine;
using System;
using FishNet.Object;

public class FirstObjectNotifier : NetworkBehaviour
{
    public static event Action<Camera> OnFirstObjectSpawned;

    [SerializeField] private Camera cam;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner) 
        {
            NetworkObject networkObject = base.LocalConnection.FirstObject;
            if (networkObject == base.NetworkObject)
                OnFirstObjectSpawned?.Invoke(cam);
        }
    }
}
