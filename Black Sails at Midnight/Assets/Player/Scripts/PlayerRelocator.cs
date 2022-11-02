using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerRelocator : MonoBehaviour
{
    public void MovePlayer(Transform targetPosition)
    {
        PlayerMovement FPController = FindObjectOfType<PlayerMovement>();
        FPController.transform.position = targetPosition.position;
        FPController.transform.rotation = targetPosition.rotation;
        FPController.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}