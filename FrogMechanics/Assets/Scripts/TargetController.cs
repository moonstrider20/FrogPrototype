/*Amorina Tabera
    This code is to enable having a targeting mechanic as well has having a visual 
        representation of the target so the player knows what they are interacting with.
    This code is placed on a UI image gameObject in the canvas. You should only need to put
        Froskr's tongue in Tongue, and the other stuff should be done on its own. 
    Uses new Unity Input system for player inputs for targeting
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TargetController : MonoBehaviour, PlayerInput.IGrappleActions
{
    Camera cam;                 //Variable to hold the camera
    public TargetInView target; //Hold the target that has the TargetInView.cs attached to it
    public Image image;         //To hold the image element of the gameObject

    public bool lockedOn;       //To check if player is locked on the target

    public int lockedTarget;    //Hold indext in list of what target is locked on

    public static List<TargetInView> nearByTargets = new List<TargetInView>();  //list to hold all targets in camera view

    public Tongue Tongue;       //Hold Tongue.cs from Froskr's tongue


    public PlayerInput input;   //Player input for the inputActions asset

    //****************************************************************************************
    //Enabel the input system
    public void OnEnable()
    {
        if (input == null)
        {
            input = new PlayerInput();

            input.Grapple.SetCallbacks(this);
        }

        input.Grapple.Enable();
    }

    public void OnDisable()
    {
        input.Grapple.Disable();
    }

    //Check if player enables targets
    public void OnToggle(InputAction.CallbackContext context)
    {
        //If not locked on, means targeting is not enabled
        //So if this is true when button press, toggle into targeting
        if (!lockedOn)
        {
            if (nearByTargets.Count >= 1)           //If more than one target is in camera view
            {
                lockedOn = true;                    //Locked on is true, on that target
                image.enabled = true;               //Target image appears to visually aid player
                                                    //and signal the targeting system is working

                lockedTarget = 0;                   //List index begins at 0
                target = nearByTargets[lockedTarget];
            }
        }

        //Targeting was already enabled, so toggle off
        else if (lockedOn)
        {
            lockedOn = false;       //no longer locked on anything
            image.enabled = false;  //targeting visual aid disappears
            lockedTarget = 0;       //lockedTarget is back at 0 index
            target = null;          //target is empty
        }
    }
    //****************************************************************************************************

    //To switch targets while the targeting mechanics is enabled
    public void OnSwitch(InputAction.CallbackContext context)
    {
        if (lockedOn)                                   //if already locked on something
        {
            if (lockedTarget == nearByTargets.Count - 1)//if only one target in view
            {
                lockedTarget = 0;
                target = nearByTargets[lockedTarget];
            }
            //other targets are in view, progress list
            else
            {
                lockedTarget++;
                target = nearByTargets[lockedTarget];
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;                          //get the main camera
        image = gameObject.GetComponent<Image>();   //get the image

        image.enabled = false;                      //image false so no one else would see
        lockedOn = false;                           //not currently locked on something
        lockedTarget = 0;                           //waiting at beginning of list
    }

    // Update is called once per frame
    void Update()
    {
        if (nearByTargets.Count == 0)
        {
            lockedOn = false;
            image.enabled = false;
            lockedTarget = 0;
            target = null;
        }

        if (lockedOn)
        {
            target = nearByTargets[lockedTarget];
            Tongue.targetInSight = true;
            gameObject.transform.position = cam.WorldToScreenPoint(target.transform.position);
            Tongue.targetPos = target.transform.position;
        }

        else
        {
            Tongue.targetInSight = false;
        }
    }
}