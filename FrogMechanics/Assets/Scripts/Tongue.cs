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
    private Rigidbody rb;

    public Vector3 targetPos;
    public bool targetInSight;

    public TargetController TargetController;
    public PlayerController PlayerController;

    //*********************
    //Tongue sound
    public AudioSource audioSource;
    public AudioClip tongue;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void LateUpdate()
    {
        DrawRope();
    }

    public void StartGrapple()
    {
        if (targetInSight)
        {
            audioSource.PlayOneShot(tongue, 0.5f);

            PlayerController.grappeling = true;
            PlayerController.Rigid.freezeRotation = true;
            //PlayerController.SetInAir();
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
        }
    }

    public void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
        Destroy(rb);
        PlayerController.grappeling = false;
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