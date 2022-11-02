using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerLookBehaviour : MonoBehaviour
{
    private PlayerMovement FPController;
    private Camera FPCamera;

    // Monobahaviour Methods
    public void Start() {
        FPController = GetComponentInChildren<PlayerMovement>();
        FPCamera = Camera.main;
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

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, FPController.transform.up);
        Debug.Log(targetRotation);

        FPController.GetComponentInChildren<Camera>().transform.rotation = new Quaternion(targetRotation.x, targetRotation.y, 0, targetRotation.w);
    }
}
