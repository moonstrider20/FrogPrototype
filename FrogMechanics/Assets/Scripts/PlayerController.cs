/*Amorina Tabera
    This code is meant to go on the player. 
    Try to leave it with what directly has to do with the player, like inputs and the basic mechanics.
    Since this is for Froskr, it handles player inputs to enable basic movement, jumping, wall walking,
        when to call grappeling, animations, and so on.
    This code is dependent on the new Unity Input System. 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerInput.IPlayerActions
{
    public static int totalArtifacts;

    [Header("Menu Stuff Here!")]
    public static bool GameIsPaused = false;    //Place to hold status if game is paused
    public GameObject pauseMenuUI;              //Object to hold pause menu

    //Class to hold the different states Froskr is in
    //This one has variabels that represent if Froskr is on the ground or in the air
    public enum WorldState
    {
        Grounded,                       //Froskr is on the ground
        InAir,                          //Froskr is in the air
    }

    public PlayerInput input;           //Place to call inputActions
    public Vector2 motion;              //Vector2 to hold direction of player movement from arrows/wasd/joystick ect.
    public Tongue tongue;               //Hold the script tongue on Froskr's tongue in the hierarchy
    public PlayerController Frog;       //Hold the player, drag Froskr from the hierarchy
    public bool canMove;                //Place to hold status if player can move (when talking)
    public VectorValue startingPosition;//Place to hold starting position in scene, drag from assets
    public Animator animator;           //Place to hold animation component


    public WorldState States;           //Place to hold what state Froskr is in
    private Transform Cam;              //Hold position of main Camera
    private Transform CamY;             //Hold position of main Camera pivot point
    private CameraFollow CamFol;        //Call CameraFollow script on the main Camera

    private DetectCollision Colli;      //Call script DetectCollision

    public Rigidbody Rigid;             //Place to hold rigidbody of the sphere

    float delta;                        //To hold current time
    public bool grappeling = false;     //Determine if Froskr is grappeling

    [Header("Physics")]
    public Transform[] GroundChecks;            //Check the surface Froskr is on
    public float DownwardPush;                  //Push Froskr against a surface to stick
    public float GravityAmt;                    //The pull of "local/world" gravity
    public float GravityBuildSpeed;             //How quickly Froskr builds gravity speed
    private float ActGravAmt;                   //The actual gravity applied to Froskr at the moment

    public LayerMask WallStick;                 //What layers the ground can be
    public float GravityRotationSpeed = 10f;    //How fast Froskr rotates to a new gravity direction

    [Header("Stats")]
    public float Speed = 15f;                   //Max speed for basic movement
    public float Acceleration = 4f;             //Acceleration to Speed
    public float turnSpeed = 2f;                //Turning speed (no clipping but physcially turn)
    private Vector3 MovDirection;               //Direction Froskr moves
    private Vector3 movepos;                    //Position Froskr moves to
    private Vector3 targetDir;                  //Target direction of Froskr
    private Vector3 GroundDir;                  //Ground direction

    [Header("Jumps")]
    public float JumpAmt;                       //Jump amount
    private bool HasJumped;                     //Check if Froskr has jumped

    [Header("Audio")]
    public AudioSource audioSource;             //Assign an audio source, can be Froskr himself
    public AudioClip walking;                   //To hold walking audio clip, dragged from assets

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(totalArtifacts);

        Rigid = GetComponentInChildren<Rigidbody>();    //Get sphere rigidbody
        Colli = GetComponent<DetectCollision>();        //Get DetectCollision script
        animator = GetComponentInChildren<Animator>();  //Get Animator component
        GroundDir = transform.up;                       //Start with the Ground Direction in the positive direciton of the y-axis
        SetGrounded();                                  //Set state to grounded

        Cam = GameObject.FindGameObjectWithTag("MainCamera").transform;     //Get the main camera
        CamY = Cam.transform.parent.parent.transform;                       //Get the pivot
        CamFol = Cam.GetComponentInParent<CameraFollow>();                  //Get the CameraFollow script

        Rigid.transform.parent = null;                                      //Detach the sphere rigidbody so it can move around freely

        Cursor.lockState = CursorLockMode.Locked;                           //Keep cursor in the center, not wandering
        canMove = true;                                                     //Player can move right away
        Rigid.transform.position = startingPosition.initalValue;            //Set player to position according to scene
    }

    //*******************************************************************************************************************************************************************

    //Method to enable the inputActions called PlayerInput
    public void OnEnable()
    {
        if (input == null)                      //Good coding practice
        {
            input = new PlayerInput();          //set input to PlayerInput()

            input.Player.SetCallbacks(this);    //Enable the callbacks
        }

        input.Player.Enable();                  //Enable input
    }

    //Method to disable the inputActions called PlayerInput
    public void OnDisable()
    {
        input.Player.Disable();                 //Disable input
    }

    //Method to call Move 
    public void OnMove(InputAction.CallbackContext context)
    {
        motion = context.ReadValue<Vector2>();
    }

    //Method to call Tongue
    public void OnTongue(InputAction.CallbackContext context)
    {
        if (context.started)                        //If player presses grapple button
        {
            tongue.StartGrapple();                  //Call method StartGrapple from the tongue script
        }

        else if (context.canceled && grappeling)    //If player stops pressing grappple button and was grappeling
        {
            tongue.StopGrapple();                   //Call method StopGrapple form the tongue script
            animator.SetInteger("State", 2);
        }
    }

    //Method to call Jump
    public void OnJump(InputAction.CallbackContext context)
    {
        if (States == WorldState.Grounded)          //Check if Froskr is on the ground, no double jumps
        {
            if (!HasJumped)                         //If Froskr has not jumped yet
            {

                StopCoroutine(JumpUp(JumpAmt));     //Stop Coroutine for jumping
                StartCoroutine(JumpUp(JumpAmt));    //Start Coroutine for jumping
                return;
            }
        }
    }

    //Method to call Quit
    public void OnQuit(InputAction.CallbackContext context)
    {
        if (GameIsPaused)       //If esc was pressed and game is already paused
            Resume();           //then resume the game
        else
            Pause();            //Else pause the game was paused already, so resume
    }

    //*******************************************************************************************************************************************************

    //Update is called once per frame
    //Good for checking player inputs
    private void Update()
    {
        //Check if Froskr is grappeling
        if (grappeling)
        {
            Rigid.position = transform.position;    //sphere follows Froskr 
            SetInAir();                             //Set Froskr's state to inAir
            FallingCtrl(delta, Speed, Acceleration);//Call method FallingCtrl
            animator.SetInteger("State", 4);
        }

        //Froskr is NOT grappeling
        else
        {
            transform.position = Rigid.position;    //Froskr follows sphere
            //animator.SetInteger("State", 2);
        }
    }

    //FixedUpdate is called any number of times per frame depending on framerate
    //Good for movement/physics
    void FixedUpdate()
    {
        //If Froskr is not allowed to move (due to talking), motion values set to zero
        if (!canMove)
        {
            motion.x = 0;
            motion.y = 0;
        }

        delta = Time.deltaTime;             //hold Time.deltaTime, easier for passing through methods and less typing

        //If Froskr is on the ground
        if (States == WorldState.Grounded)
        {
            float Spd = Speed;              //Make a variable to hold Froskr's current speed, set Froskr's speed to normal

            //If player is not trying to move Froskr
            if (motion.x == 0 && motion.y == 0)
            {
                Spd = 0f;                   //Set Froskr's speed to zero
            }


            MoveSelf(delta, Spd, Acceleration); //Call method MoveSelf to move Froskr accordingly

            //Check if Froskr is on the ground
            bool Ground = Colli.CheckGround(-GroundDir);

            //If Froskr is not on the ground
            if (!Ground)
            {
                SetInAir();     //Set Froskr's state to inAir
                return;         //return now so we don't accidently go into the else if statement below
            }

        }

        //Check if Froskr is in the air
        else if (States == WorldState.InAir)
        {
            //Check if Froskr jumped
            if (HasJumped)
                return;     //only return when jump time is done

            FallingCtrl(delta, Speed, Acceleration);        //When jump time done, call method FallingCtrl

            //Check if Froskr hit the ground
            bool Ground = Colli.CheckGround(-GroundDir);

            //If Froskr hit the ground
            if (Ground)
            {
                Rigid.useGravity = false;   //Don't use local world gravity anymore
                SetGrounded();              //Set Froskr's state to ground
                animator.SetInteger("State", 0);
                return;
            }
        }
    }

    //Transition Froskr to the ground
    public void SetGrounded()
    {
        ActGravAmt = 5f; //our gravity is returned to normal

        States = WorldState.Grounded;
    }

    //Transition Froskr to the air
    public void SetInAir()
    {
        States = WorldState.InAir;
    }

    //Froskr jumps up
    IEnumerator JumpUp(float UpwardsAmt)
    {
        HasJumped = true;                                                       //Froskr has jumped

        SetInAir();                                                             //Set Froskr's state to inAir

        //Good coding practice
        if (UpwardsAmt != 0)
            Rigid.AddForce((transform.up * UpwardsAmt), ForceMode.Impulse);     //Froskr jumps

        yield return new WaitForSecondsRealtime(0.3f);
        HasJumped = false;
        Rigid.useGravity = true;
    }

    //Check the angle of the floor Froskr is standing on
    Vector3 FloorAngleCheck()
    {
        //RaycastHit is a structure, make three total, once for each groundCheck
        RaycastHit HitFront;
        RaycastHit HitCentre;
        RaycastHit HitBack;

        //Physics.Raycast returns a bool, we use 'out' for the RaycastHit variables since they are structures
        //Physics.Racast(origin"starting point", direction"direction of ray", hitInfo"contains more info if returns true", maxDistance, layerMask)
        //More on 'out'- so if returns true, the RaycastHit structure is filled out with the information about what what hit. 
        Physics.Raycast(GroundChecks[0].position, -GroundChecks[0].transform.up, out HitFront, 10f, WallStick);
        Physics.Raycast(GroundChecks[1].position, -GroundChecks[1].transform.up, out HitCentre, 10f, WallStick);
        Physics.Raycast(GroundChecks[2].position, -GroundChecks[2].transform.up, out HitBack, 10f, WallStick);

        Vector3 HitDir = transform.up;      //create a Vector3 in the positive direciton of the y-axis

        //Get the normals of the surface Froskr is on from each groundCheck
        //Make sure there was something hit by the raycast and then get the normal
        if (HitFront.transform != null)
        {
            HitDir += HitFront.normal;
        }
        if (HitCentre.transform != null)
        {
            HitDir += HitCentre.normal;
        }
        if (HitBack.transform != null)
        {
            HitDir += HitBack.normal;
        }

        //returnt the overall direction
        return HitDir.normalized;
    }

    //move our character
    void MoveSelf(float d, float Speed, float Accel)
    {
        bool MoveInput = false;                                 //Set getting input to false

        Vector3 screenMovementForward = CamY.transform.forward; //Get what direction camera is facing
        Vector3 screenMovementRight = CamY.transform.right;     //Get what direction is "right" according to the camera (Vector3(1,0,0))

        Vector3 h = screenMovementRight * motion.x;             //Get actual horizontal movment
        Vector3 v = screenMovementForward * motion.y;           //Get actual walking forwards or backwards

        Vector3 moveDirection = (v + h).normalized;             //Just have direction of player movement

        //If player is not moving Froskr
        if (motion.x == 0 && motion.y == 0)
        {
            targetDir = transform.forward;                      //targetDir set to where Froskr is facing (blue, positive z-axis direction according to Froskr)
            animator.SetInteger("State", 0);
        }
        //Player is moving Froskr
        else
        {
            targetDir = moveDirection;                          //targetDir set to direction Player wants to move Froskr
            MoveInput = true;                                   //Player has inputs
            animator.SetInteger("State", 1);
        }

        Quaternion lookDir = Quaternion.LookRotation(targetDir);    //Set Froskr's look direction to the targetDir
        float TurnSpd = turnSpeed;                                  //Make a variabel to hold the turning speed

        Vector3 SetGroundDir = FloorAngleCheck();                   //Find out what direction the ground is
        GroundDir = Vector3.Lerp(GroundDir, SetGroundDir, d * GravityRotationSpeed);    //

        //lerp mesh slower when not on ground
        RotateSelf(SetGroundDir, d, GravityRotationSpeed);
        RotateMesh(d, targetDir, TurnSpd);

        //moving character
        float Spd = Speed;
        Vector3 curVelocity = Rigid.velocity;

        MovDirection = transform.forward;


        Vector3 targetVelocity = MovDirection * Spd;

        //push downwards in downward direction of mesh
        targetVelocity -= SetGroundDir * DownwardPush;

        Vector3 dir = Vector3.Lerp(curVelocity, targetVelocity, d * Accel);
        Rigid.velocity = dir;                                               //Froskr FINALLY MOVES at this line
    }

    //move once we are in air
    void FallingCtrl(float d, float Speed, float Accel)
    {
        float _xMov = motion.x;
        float _zMov = motion.y;

        Vector3 screenMovementForward = CamY.transform.forward;
        Vector3 screenMovementRight = CamY.transform.right;

        Vector3 h = screenMovementRight * _xMov;
        Vector3 v = screenMovementForward * _zMov;

        Vector3 moveDirection = (v + h).normalized;

        if (_xMov != 0 || _zMov != 0)
        {
            targetDir = moveDirection;
        }
        else
        {
            targetDir = transform.forward;
        }

        Quaternion lookDir = Quaternion.LookRotation(targetDir);

        //lerp mesh slower when not on ground
        RotateSelf(GroundDir, d, GravityRotationSpeed);
        RotateMesh(d, transform.forward, turnSpeed);

        //move character
        MovDirection = targetDir;
        float Spd = Speed;
        Vector3 curVelocity = Rigid.velocity;

        Vector3 targetVelocity = MovDirection;

        //fall from the air
        if (ActGravAmt < GravityAmt - 0.5f)
            ActGravAmt = Mathf.Lerp(ActGravAmt, GravityAmt, GravityBuildSpeed * d);

        //move either upwards or downwards with gravity
        targetVelocity -= GroundDir * ActGravAmt;

        Vector3 dir = Vector3.Lerp(curVelocity, targetVelocity, d * Accel);
        Rigid.velocity = dir;
    }
    //rotate the direction we face down
    void RotateSelf(Vector3 Direction, float d, float GravitySpd)
    {
        Vector3 LerpDir = Vector3.Lerp(transform.up, Direction, d * GravitySpd);
        transform.rotation = Quaternion.FromToRotation(transform.up, LerpDir) * transform.rotation;
    }
    //rotate the direction we face forwards
    void RotateMesh(float d, Vector3 LookDir, float spd)
    {
        Quaternion SlerpRot = Quaternion.LookRotation(LookDir, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, SlerpRot, spd * d);
    }

    //Method to resume game when paused
    void Resume()
    {
        pauseMenuUI.SetActive(false);               //enable pauseMenuUI
        Time.timeScale = 1f;                        //Resume time
        GameIsPaused = false;                       //Game no longer paused
        Cursor.lockState = CursorLockMode.Locked;   //Relock the cursro for player
    }

    //Method to pause game while playing
    void Pause()
    {
        pauseMenuUI.SetActive(true);                //disable the pauseMenuUI
        Time.timeScale = 0f;                        //Pause time
        GameIsPaused = true;                        //Game is now paused
        Cursor.lockState = CursorLockMode.None;     //Unlock the cursor for player
    }
}