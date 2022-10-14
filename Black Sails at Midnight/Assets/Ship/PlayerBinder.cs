using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.AI;

public class PlayerBinder : MonoBehaviour
{
    [SerializeField]
    public float lostContactTimer = 1f;

    [SerializeField]
    public GameObject parentedPlayer;

    private float timeOfLastContact;

    // Monobahaviour Methods
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
            FPController.m_CharacterController.SimpleMove(gameObject.GetComponent<NavMeshAgent>().velocity);

            if (Time.realtimeSinceStartup - timeOfLastContact > lostContactTimer)
            {
                FPController.enableJumpSound = true;
                FPController.m_UseHeadBob = true;

                parentedPlayer = null;
            }
        }
    }

    // Public Methods
    public void Unbind()
    {
        parentedPlayer = null;
    }

    public void Bind(GameObject player)
    {
        parentedPlayer = player;
        timeOfLastContact = Time.realtimeSinceStartup;
    }
}
