using FishNet;
using FishNet.Object;
using FishNet.Object.Prediction;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputDriver : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerBody;
    private CharacterController _characterController;
    private Vector2 _moveInput;
    private bool _jump;
    [SerializeField] public float jumpSpeed = 6f;
    [SerializeField] public float speed = 8f;
    [SerializeField] public float gravity = -9.8f;

    #region Types.

    public struct MoveInputData
    {
        public Vector2 moveVector;
        public bool jump;
        public bool grounded;
    }

    public struct ReconcileData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Quaternion PlayerBodyRot;
        public ReconcileData(Vector3 position, Quaternion rotation, Quaternion playerBodyRot)
        {
            Position = position;
            Rotation = rotation;
            PlayerBodyRot = playerBodyRot;

        }
    }

    #endregion
    private void Start()
    {
        _characterController = GetComponent(typeof(CharacterController)) as CharacterController;
        _jump = false;
        InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;
    }

    #region UnityEventCallbacks
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (!base.IsOwner)
            return;
        _moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!base.IsOwner)
            return;
        if (context.started || context.performed)
        {
            _jump = true;
        }
        else if (context.canceled)
        {
            _jump = false;
        }
    }
    #endregion

    #region Prediction Methods

    [Replicate]
    private void Move(MoveInputData md, bool asServer, bool replaying = false)
    {
        Vector3 move = transform.right * md.moveVector.x + transform.forward * md.moveVector.y;
        if (md.grounded)
        {
            move.y = gravity;
            
            if (md.jump)
            {
                move.y = jumpSpeed;
            }
        }
        move.y += gravity * (float)base.TimeManager.TickDelta; // gravity is negative...
        _characterController.Move(move * speed * (float)base.TimeManager.TickDelta);


        // Here I call method for character body rotation
        Look(_moveInput);
    }

    [Reconcile]
    private void Reconciliation(ReconcileData rd, bool asServer)
    {
        transform.position = rd.Position;
        transform.rotation = rd.Rotation;
        playerBody.rotation = rd.PlayerBodyRot;
    }

    #endregion

    #region Movement Processing

    private void GetInputData(out MoveInputData moveData)
    {
        moveData = new MoveInputData
        {
            jump = _jump,
            grounded = _characterController.isGrounded,
            moveVector = _moveInput
        };
    }
    private void TimeManager_OnTick()
    {
        if (base.IsOwner)
        {
            Reconciliation(default, false);
            GetInputData(out MoveInputData md);
            Animate(md);
            Move(md, false);
        }

        if (base.IsServer)
        {
            Move(default, true);
            ReconcileData rd = new ReconcileData(transform.position, transform.rotation, playerBody.rotation);
            Reconciliation(rd, true);
        }
    }

    #endregion

    private void OnDestroy()
    {
        if (InstanceFinder.TimeManager != null)
            InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
    }

    private void Animate(MoveInputData moveData) 
    {
        float x = moveData.moveVector.x;
        float y = moveData.moveVector.y;
        if (Mathf.Abs(x) + Mathf.Abs(y) < float.Epsilon)
        {
            animator.SetBool("Walking", false);
        }
        else 
        {
            animator.SetFloat("movementX", x);
            animator.SetFloat("movementY", y);
            animator.SetBool("Walking", true);

        }

    }

    //Method for rotating player Body (which is child of gameobject with character controller) to the movement direction
    private void Look(Vector2 moveVector)
    {
        Vector3 rotVector = new Vector3(moveVector.x,0,moveVector.y);
        if (Mathf.Abs(rotVector.x) + Mathf.Abs(rotVector.z) < float.Epsilon)
        {
            return;
        }
        else
        {
            playerBody.rotation = Quaternion.LookRotation(rotVector, Vector3.up);
        }

    }

}
