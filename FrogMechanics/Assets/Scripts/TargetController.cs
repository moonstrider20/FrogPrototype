using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    Camera cam;
    public TargetInView target;
    public Image image;

    public bool lockedOn;

    public int lockedTarget;

    public static List<TargetInView> nearByTargets = new List<TargetInView>();

    public Tongue Tongue;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        image = gameObject.GetComponent<Image>();

        lockedOn = false;
        lockedTarget = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !lockedOn)
        {
            if (nearByTargets.Count >= 1)
            {
                lockedOn = true;
                image.enabled = true;

                lockedTarget = 0;
                target = nearByTargets[lockedTarget];
            }
        }

        else if ((Input.GetMouseButtonDown(1) && lockedOn) || nearByTargets.Count == 0)
        {
            lockedOn = false;
            image.enabled = false;
            lockedTarget = 0;
            target = null;
        }

        if (Input.GetMouseButtonDown(2) && lockedOn)
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
        Debug.Log("lockedOn is " + lockedOn);
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
