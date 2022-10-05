using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;
using FishNet.Object.Prediction;

public class PlayerBodyRotation : NetworkBehaviour
{


    private Vector2 inputVector;

    #region Types
    public struct MoveInputData 
    {
        public Vector2 moveVector;
    
    }


    public struct ReconcileData
    { 
        public Quaternion Rotation;
        public ReconcileData(Quaternion rotation)
        {
            Rotation = rotation;
        }
    }
    #endregion

    


    // Start is called before the first frame update
    void Start()
    {
        PlayerInputDriver.OnGatherInput += PlayerInputDriver_OnGatherInput;
        InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;
    }

    private void OnDestroy()
    {
        PlayerInputDriver.OnGatherInput -= PlayerInputDriver_OnGatherInput;
        //InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
    }

    private void PlayerInputDriver_OnGatherInput(Vector2 obj)
    {
        if (!base.IsOwner)
            return;
        inputVector = obj;
    }


    #region Prediction Methods

    [Replicate]
    private void Look(MoveInputData md, bool asServer, bool replaying = false)
    {
        Vector3 rotVector = new Vector3(md.moveVector.x, 0, md.moveVector.y);
        if (Mathf.Abs(rotVector.x) + Mathf.Abs(rotVector.z) < float.Epsilon)
        {
            return;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(rotVector, Vector3.up);
        }

    }

    [Reconcile]
    private void Reconciliation(ReconcileData rd, bool asServer)
    {
        transform.rotation = rd.Rotation;
    }

    #endregion

    #region Movement Processing

    private void GetInputData(out MoveInputData moveData)
    {
        
        moveData = new MoveInputData
        {
            moveVector = inputVector
        };
    }
    private void TimeManager_OnTick()
    {
        if (base.IsOwner)
        {
            Reconciliation(default, false);
            GetInputData(out MoveInputData md);
            Look(md, false);
        }

        if (base.IsServer)
        {
            Look(default, true);
            ReconcileData rd = new ReconcileData(transform.rotation);
            Reconciliation(rd, true);
        }
    }

    #endregion

    //Method for rotating player Body (which is child of gameobject with character controller) to the movement direction
    private void Look(Vector2 moveVector)
    {
        Vector3 rotVector = new Vector3(moveVector.x, 0, moveVector.y);
        if (Mathf.Abs(rotVector.x) + Mathf.Abs(rotVector.z) < float.Epsilon)
        {
            return;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(rotVector, Vector3.up);
        }

    }
}
