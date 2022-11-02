using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    KeyCode GrappleKey = KeyCode.T;
    [SerializeField]
    float Range = 20f;
    [SerializeField]
    float MaxDistanceMulitplier = 0.8f;
    [SerializeField]
    float MinDistanceMultiplier = 0.25f;

    [Header("Joint Properties")]
    [SerializeField]
    float SpringForce = 4.5f;
    [SerializeField]
    float Dampening = 7f;
    [SerializeField]
    float MassScale = 4.5f;

    [Header("Required Components")]
    [SerializeField]
    Transform GrappleHookStart;
    [SerializeField]
    Transform camera;
    [SerializeField]
    Transform player;


    private GameObject GrapplePoint;
    private LineRenderer lineRenderer;
    public LayerMask whatIsGrappleable;
    private float maxDistance = 100f;
    private SpringJoint joint;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(GrappleKey))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp(GrappleKey))
        {
            StopGrapple();
        }
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, Range))
        {
            GrapplePoint = new GameObject("Grappling Point");
            GrapplePoint.transform.position = hit.point;
            GrapplePoint.transform.parent = hit.transform;

            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = GrapplePoint.transform.position;

            float distanceFromPoint = Vector3.Distance(player.position, GrapplePoint.transform.position);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = SpringForce;
            joint.damper = Dampening;
            joint.massScale = MassScale;

            lineRenderer.positionCount = 2;
            currentGrapplePosition = GrappleHookStart.position;
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        lineRenderer.positionCount = 0;
        Destroy(joint);
        Destroy(GrapplePoint);
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;
        joint.connectedAnchor = GrapplePoint.transform.position;
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, GrapplePoint.transform.position, Time.deltaTime * 8f); ;

        lineRenderer.SetPosition(0, GrappleHookStart.position);
        lineRenderer.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return GrapplePoint.transform.position;
    }

}
