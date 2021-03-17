using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerInput.IPlayerActions
{
    public float speed;
    public PlayerInput input;
    private Rigidbody rb;
    public Vector2 motion;
    public Vector2 lookInput;
    public float rotX = 0;
    public float rotY = 0;
    public float sensitivity;
    public float MaxLookAngle;
    public float MinLookAngle;
    public Transform playerCamera;
    public float turnSmoothTime = 1.0f;
    float turnSmoothVelocity;
    public Transform thirdCamera;
    public Tongue tongue;
    //public CinemachineInputProvider cine;
    //public Target target;

    //public State state;
    public Vector3 hookshotPosition;
    //public CharacterController characterController;
    //public enum State
    //{
      //  Normal,
       // HookshotFlyingPlayer
    //}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;           //Keep cursor in the center, not wandering
        Cursor.visible = true;                             //Make cursor invisible 
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        /*switch (state)
        {
            default:
            case State.Normal:
                Move();
                HandleHookShotStart();
                break;
            case State.HookshotFlyingPlayer:
                HandleHookshotMovement();
                break;
        }*/
    }

    public void OnEnable()
    {
        if (input == null)
        {
            input = new PlayerInput();

            input.Player.SetCallbacks(this);
        }

        input.Player.Enable();
    }

    public void OnDisable()
    {
        input.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        motion = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //lookInput = context.ReadValue<Vector2>();
    }

    public void OnTongue(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("I'm trying!!!!");
            tongue.StartGrapple();
        }

        else if (context.canceled)
        {
            Debug.Log("I stopped");
            tongue.StopGrapple();
        }
    }

    public void OnHookshot(InputAction.CallbackContext context)
    {
        //HandleHookShotStart();
        //Debug.Log("Rightmouse key");
        if (context.started)
        {
            input.Player.Look.Disable();
            //cine.XYAxis = none;
            Cursor.lockState = CursorLockMode.None;
        }

        else if (context.canceled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            input.Player.Look.Enable();
        }
    }

    void Move()
    {
        Vector3 movement = new Vector3(motion.x, 0.0f, motion.y).normalized;

        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + thirdCamera.eulerAngles.y; ;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f); //targetAngle w/ angle (???)

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
            rb.AddForce(moveDirection.normalized * speed);// Time.deltaTime);
            //characterController.Move(movement * Time.deltaTime * speed);                   //Move the Player
        }
    }

    //*******
    private void HandleHookShotStart()
    {
        Debug.Log("Hookshot START!");
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit))
        {
            hookshotPosition = raycastHit.point;
            //state = State.HookshotFlyingPlayer;
            HandleHookshotMovement();
        }

    }

    private void HandleHookshotMovement()
    {
        Debug.Log("MOVE HOOK!");
        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        float hookshotSpeed = 5f;

        //characterController.Move(hookshotDir * hookshotSpeed * Time.deltaTime);
        //rb.transform.Translate(hookshotDir * hookshotSpeed * Time.deltaTime);
        rb.AddForce(hookshotDir * hookshotSpeed);
    }
}
