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

    [SerializeField]
    LayerMask boundLayer;

    int defaultLayer;

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
            defaultLayer = boundPlayer.gameObject.layer;

            PlayerMovement FPController = boundPlayer.GetComponent<PlayerMovement>();            
            FPController.transform.parent = this.gameObject.transform;

            FPController.gameObject.layer = Mathf.RoundToInt(Mathf.Log(boundLayer.value, 2));
        }

        timeOfLastContact = Time.realtimeSinceStartup;
    }

    public void Unbind()
    {
        if (boundPlayer != null)
        {
            PlayerMovement FPController = boundPlayer.GetComponent<PlayerMovement>();
            FPController.gameObject.transform.parent = null;
            FPController.gameObject.layer = defaultLayer;

            boundPlayer = null;
        }
    }

    public bool IsBound()
    {
        return boundPlayer != null;
    }   
}
