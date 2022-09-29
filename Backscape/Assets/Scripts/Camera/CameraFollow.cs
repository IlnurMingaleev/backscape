using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : NetworkBehaviour
{
    [SerializeField] private Transform target;
    private void Update()
    {
        if (!base.IsOwner)
            return;
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

}