using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.AI;

public class PlayerBinder : MonoBehaviour, IBindingSurface
{
    [SerializeField]
    public float lostContactTimer = 1f;

    [SerializeField]
    private GameObject boundPlayer;
    private float timeOfLastContact;

    private void Update() 
    {
        if (boundPlayer != null)
        {
            FirstPersonController FPController = boundPlayer.GetComponent<FirstPersonController>();
            FPController.m_CharacterController.SimpleMove(GetComponent<NavMeshAgent>().velocity);

            if (Time.realtimeSinceStartup - timeOfLastContact > lostContactTimer)
            {
                FPController.enableJumpSound = true;
                FPController.m_UseHeadBob = true;

                Unbind();
            }
        }
    }

    public void Bind(GameObject source)
    {
        Debug.Log("Bound!");
        if (source.GetComponentInChildren<FirstPersonController>() == null)
            return;

        if (boundPlayer == null)
        {
            boundPlayer = source.GetComponentInChildren<FirstPersonController>().gameObject;
            timeOfLastContact = Time.realtimeSinceStartup;

            FirstPersonController FPController = boundPlayer.GetComponent<FirstPersonController>();
            FPController.enableJumpSound = false;
            FPController.m_UseHeadBob = false;
        }
    }

    public void Unbind()
    {
        Debug.Log("Unbind");
        boundPlayer = null;
    }

    public bool IsBound()
    {
        return boundPlayer != null;
    }
}
