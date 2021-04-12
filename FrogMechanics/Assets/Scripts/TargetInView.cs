using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInView : MonoBehaviour
{
    Camera cam;
    bool addOnlyOnce;
    int grappleDistance = 100;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        addOnlyOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = cam.WorldToViewportPoint(gameObject.transform.position);

        bool onScreen = targetPosition.x > 0 && targetPosition.x < 1 && targetPosition.y > 0 && targetPosition.y < 1 && targetPosition.z > 0 && targetPosition.z < grappleDistance;//targetPosition.z > 0 && targetPosition.x > 0 && targetPosition.x < 1 && targetPosition.y > 0 && targetPosition.y < 1;

        if (onScreen && addOnlyOnce)
        {
            addOnlyOnce = false;
            TargetController.nearByTargets.Add(this);
        }

        if (!onScreen && !addOnlyOnce)
        {
            addOnlyOnce = true;
            TargetController.nearByTargets.Remove(this);
        }
    }
}