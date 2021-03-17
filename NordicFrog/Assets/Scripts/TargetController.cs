using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    /*TargetInView target;
    public Image image;
    bool lockedOn;
    int lockedTarget;
    public static List<TargetInView> nearByTargets = new List<TargetInView>();


    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        lockedOn = false;
        lockedTarget = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !lockedOn)
        {
            if (nearByTargets.Count >= 1)
            {
                lockedOn = true;
                image.enabled = true;
                lockedTarget = 0;
                target = nearByTargets[lockedTarget];
            }
        }

        else if ((Input.GetKeyDown(KeyCode.Space) && lockedOn) || nearByTargets.Count == 0)
        {
            lockedOn = false;
            image.enabled = false;
            lockedTarget = 0;
            target = null;
        }

        if (Input.GetKeyDown(KeyCode.X))
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

            if (lockedOn)
            {
                target = nearByTargets[lockedTarget];
                //gameObject.transform.position = thirdCamera.WorldToScreenPoint(target.transform.position);
                //gameObject.transform.Rotate(new Vector3(0, 0, -1));

            }
        }
    }*/
}
