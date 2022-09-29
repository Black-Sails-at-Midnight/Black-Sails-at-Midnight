using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.AI;

public class PlayerBinder : MonoBehaviour
{
    [SerializeField]
    public float lostContactTimer = 1f;

    private GameObject parentedPlayer;
    private float timeOfLastContact;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.transform.parent != null && other.gameObject.transform.parent.gameObject.tag == "Player")
        {
            timeOfLastContact = Time.realtimeSinceStartup;
            parentedPlayer = other.gameObject;

            FirstPersonController FPController = parentedPlayer.GetComponent<FirstPersonController>();
            FPController.enableJumpSound = false;
            FPController.m_UseHeadBob = false;
        }
    }

    private void OnCollisionStay(Collision other) {
        timeOfLastContact = Time.realtimeSinceStartup;
    }


    private void Update() {
        if (parentedPlayer != null)
        {
            FirstPersonController FPController = parentedPlayer.GetComponent<FirstPersonController>();
            FPController.m_CharacterController.SimpleMove(transform.forward.normalized * gameObject.GetComponent<NavMeshAgent>().speed);

            if (Time.realtimeSinceStartup - timeOfLastContact > lostContactTimer)
            {
                FPController.enableJumpSound = true;
                FPController.m_UseHeadBob = true;

                parentedPlayer = null;
            }
        }
    }
}
