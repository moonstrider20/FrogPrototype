//Amorina Tabera
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Inputs.IPlayerActions
{
    public float speed;                     //Speed of Player
    public Inputs controls;                 //Gets Input System Controls from script called Inputs
    public Vector2 motion;                  //Vector for movement of Player
    public Vector2 mouseInput;              //Vector for looking around for Player

    //SerializedField makes this variable visible in the unity editor while still maintaining privacy, need to set for other vairables
    [SerializeField] private CharacterController controller = null; //Get Character Controller of Player, apparently better than rigidbody when not wanting to deal with physics

    public float sensitivity;               //Mouse sensitivity
    public float MaxLookUpAngle;            //How high Player can look
    public float MinLookUpAngle;            //How low Player can look

    public float rotX = 0, rotY = 0;        //Rotations for camera

    public Transform ffCamera;              //To hold the FFCamera

    Vector3 movement;                       //Vector to hold movment
    Vector3 level;
    public bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();   //Get the Character Controller component of the player
        Cursor.lockState = CursorLockMode.Locked;           //Keep cursor in the center, not wandering
        Cursor.visible = false;                             //Make cursor invisible 
    }

    //To enable the Input System contorls for "Player"
    public void OnEnable()
    {
        //safety thing
        if (controls == null)
        {
            controls = new Inputs();
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
    }

    //To disable the Input System controls for "Player"
    public void OnDisable()
    {
        controls.Player.Disable();
    }

    //Inputsystem.inputActioins.Player.Move.triggered
    public void OnMovement(InputAction.CallbackContext context)
    {
        motion = context.ReadValue<Vector2>();                      //Get the values of the directions Player is moving (arrows, WASD, ect)
    }

    //Inputsystem.inputActions.Player.Look.triggered
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();                  //read values of the mouse(Look) motion
    }

    public void OnQuit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        if (!pressed)
        {
            pressed = true;
            level = transform.up;
        }

        else
            pressed = false;
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        if (!pressed)
        {
            pressed = true;
            level = -transform.up;
        }

        else
            pressed = false;

    }

    void Update()
    {
        Move();         //Calls method Move()
        Turn();         //Calls method Turn()
    }

    //Method to move (should be in OnMovement but didn't work for some reason
    private void Move()
    {
        if (pressed)
        {
            movement = transform.right * motion.x + level + transform.forward * motion.y; // GLOBAL[new Vector3(motion.x, 0.0f, motion.y);] <- that but relative to the direction of the camera
        }
        else
            movement = transform.right * motion.x + transform.forward * motion.y;
        controller.Move(movement * Time.deltaTime * speed);                   //Move the Player
    }

    //Method to look around (should be in OnLook, but left here since the movement didn't work)
    private void Turn()
    {
        rotX += mouseInput.x * sensitivity * Time.deltaTime;                            //Get horizontal input
        rotY += mouseInput.y * sensitivity * Time.deltaTime;                            //Get vertical input
        rotY = Mathf.Clamp(rotY, MinLookUpAngle, MaxLookUpAngle);                       //Restrict how far up and down Player can look

        ffCamera.transform.localRotation = Quaternion.Euler(-rotY, mouseInput.x, 0f);   //Move camera with mouse
        transform.Rotate(Vector3.up * mouseInput.x);                                    //Move Player with camera
    }       
}