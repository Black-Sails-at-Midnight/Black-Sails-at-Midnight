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
            if (Time.realtimeSinceStartup - timeOfLastContact > lostContactTimer)
            {
                Unbind();
            }
        }
    }

    public void Bind(GameObject source)
    {
        if (source.GetComponentInChildren<PlayerMovement>() == null || source.GetComponentInChildren<PlayerMovement>().disableMovement)
            return;

        if (boundPlayer == null)
        {
            boundPlayer = source.GetComponentInChildren<PlayerMovement>().gameObject;
            timeOfLastContact = Time.realtimeSinceStartup;

            PlayerMovement FPController = boundPlayer.GetComponent<PlayerMovement>();
            FPController.transform.parent = this.gameObject.transform;
        }
    }

    public void Unbind()
    {
        PlayerMovement FPController = boundPlayer.GetComponent<PlayerMovement>();
        FPController.transform.parent = null;

        boundPlayer = null;
    }

    public bool IsBound()
    {
        return boundPlayer != null;
    }
}
