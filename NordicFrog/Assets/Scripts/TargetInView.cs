using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInView : MonoBehaviour
{
   /* Camera cam;
    bool addOnlyOnce;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        addOnlyOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPostition = cam.WorldToViewportPoint(gameObject.transform.position);

        bool onScreen = targetPostition.z > 0 && targetPostition.x > 0 && targetPostition.x < 1 && targetPostition.y > 0 && targetPostition.y < 1;

        if (onScreen && addOnlyOnce)
        {
            addOnlyOnce = false;
            TargetController.nearByTargets.Add(this);
        }
    }*/
}
