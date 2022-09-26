using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class IsometricPlayerController : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerBody;
    public CharacterController controller;
    public float speed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private Vector3 velocity;
    private bool isGrounded;
    //private Quaternion newRotation;


    public AudioClip footStepSound;
    public float footStepDelay;

    private float nextFootstep = 0;

    private Vector3 input;
    /*public override void OnNetworkSpawn() 
    {
        
    }*/
    private void Start()
    {
        //newRotation = playerBody.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLocalPlayer)
        {
            return;
        }
        GatherInput();
        CheckIsGrounded();
        Move();
        Jump();
        PlayFootsteps();
        Look();
       
    }
    private void Move() 
    {
        
        if (Mathf.Abs(input.x) + Mathf.Abs(input.z) < float.Epsilon)
        {
            animator.SetBool("Walking", false);
        }
        else
        {
            animator.SetBool("Walking", true);
        }

        Vector3 motion = transform.right * input.x + transform.forward * input.z;

        //animator.SetFloat("movementX", input.x);
        //animator.SetFloat("movementY", input.y);
        controller.Move(motion * speed * Time.deltaTime);

        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void GatherInput() 
    {
        input = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));

        
    }
    private void Jump() 
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void PlayFootsteps() 
    {

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) && isGrounded)
        {
            nextFootstep -= Time.deltaTime;
            if (nextFootstep <= 0)
            {
                GetComponent<AudioSource>().PlayOneShot(footStepSound, 0.7f);
                nextFootstep += footStepDelay;
            }
        }
    }
    private void CheckIsGrounded() 
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void Look() 
    {
        if (Mathf.Abs(input.x) + Mathf.Abs(input.z) < float.Epsilon)
        {
            return;
        }
        else
        {
            playerBody.rotation = Quaternion.LookRotation(input, Vector3.up);
        }
    
    }
}
