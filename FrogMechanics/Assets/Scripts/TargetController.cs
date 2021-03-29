using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TargetController : MonoBehaviour, PlayerInput.IGrappleActions
{
    Camera cam;
    public TargetInView target;
    public Image image;

    public bool lockedOn;

    public int lockedTarget;

    public static List<TargetInView> nearByTargets = new List<TargetInView>();

    public Tongue Tongue;


    public PlayerInput input;

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

    public void OnToggle(InputAction.CallbackContext context)
    {
        if (!lockedOn)
        {
            if (nearByTargets.Count >= 1)
            {
                lockedOn = true;
                image.enabled = true;

                lockedTarget = 0;
                target = nearByTargets[lockedTarget];
            }
        }

        else if (lockedOn)
        {
            lockedOn = false;
            image.enabled = false;
            lockedTarget = 0;
            target = null;
        }
    }

    public void OnSwitch(InputAction.CallbackContext context)
    {
        if (lockedOn)
        {
            if (lockedTarget == nearByTargets.Count - 1)
            {
                lockedTarget = 0;
                target = nearByTargets[lockedTarget];
            }
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
        cam = Camera.main;
        image = gameObject.GetComponent<Image>();

        image.enabled = false;
        lockedOn = false;
        lockedTarget = 0;
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
            /*Debug.Log(target.transform.position);*/
            Tongue.targetPos = target.transform.position;
        }

        else
        {
            Tongue.targetInSight = false;
        }
    }
}