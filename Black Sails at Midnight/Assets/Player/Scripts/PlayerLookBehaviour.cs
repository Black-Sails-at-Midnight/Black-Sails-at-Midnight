using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerLookBehaviour : MonoBehaviour
{
    private FirstPersonController FPController;
    private Camera FPCamera;

    // Monobahaviour Methods
    public void Start() {
        FPController = GetComponentInChildren<FirstPersonController>();
        FPCamera = FPController.GetComponentInChildren<Camera>();
    }

    // Public Methods
    public void LookAtTarget(GameObject targetOpbject)
    {
        RotatePlayerTowards(targetOpbject.transform.position);
    }

    public void LookAtPosition(Vector3 targetPosition)
    {
        RotatePlayerTowards(targetPosition);
    }

    // Private Methods
    private void RotatePlayerTowards(Vector3 position)
    {
        Vector3 targetDirection = (position - FPCamera.transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, FPCamera.transform.up);
        FPController.m_MouseLook.m_CharacterTargetRot = new Quaternion(0, targetRotation.y, 0, targetRotation.w);
        FPController.m_MouseLook.m_CameraTargetRot = new Quaternion(targetRotation.x, 0, 0, targetRotation.w);
    }
}
