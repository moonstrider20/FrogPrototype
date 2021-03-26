using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform tongueTip;//, thirdCamera, 
    public Transform player;
    //private float maxDistance = 150f;
    private SpringJoint joint;

    public Vector3 targetPos;
    public bool targetInSight;

    public TargetController TargetController;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        /*if(Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }*/
    }

    void LateUpdate()
    {
        DrawRope();
    }

    public void StartGrapple()
    {
        //RaycastHit hit;
        Debug.Log("I stared grapple");
        /*if (Physics.Raycast(thirdCamera.position, thirdCamera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.spring = 3f;
            joint.damper = 5f;
            joint.massScale = 3f;

            lr.positionCount = 2;
            currentGrapplePosition = tongueTip.position;
        }*/
        Debug.Log(targetInSight);
        if (targetInSight)
        {
            grapplePoint = targetPos;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.spring = 3f;
            joint.damper = 5f;
            joint.massScale = 3f;

            lr.positionCount = 2;
            currentGrapplePosition = tongueTip.position;


            //***********************************
            TargetController.lockedOn = false;
            TargetController.image.enabled = false;
            TargetController.lockedTarget = 0;
            TargetController.target = null;
            //**********************************

            //Debug.Log("Line where you be?");
        }
        //else Debug.Log("FALSE");
    }

    public void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        if (!joint)
            return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, tongueTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappleing()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}