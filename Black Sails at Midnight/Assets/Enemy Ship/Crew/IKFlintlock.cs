using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Animator))]
public class IKFlintlock : MonoBehaviour
{
    public Transform target;
    public Transform aimTransform;
    public Transform bone;

    public int refreshRate = 4;

    private void Start()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position;

        for (int i = 0; i < refreshRate; i++)
        {
            AimAtTarget(bone, targetPosition);
        }
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        bone.rotation = aimTowards * bone.rotation;
    }
}
