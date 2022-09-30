using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WaterSurfaceCollisionHandler : MonoBehaviour
{
    [SerializeField]
    public Vector3 playerRespawnPosition;
    private void OnCollisionEnter(Collision other) {
        
        if (other.gameObject.transform.parent != null && other.gameObject.transform.parent.gameObject.tag == "Player")
        {
            FirstPersonController FPController = other.gameObject.GetComponent<FirstPersonController>();
            FPController.m_CharacterController.Move(playerRespawnPosition - FPController.transform.position);
        }
    }
}
