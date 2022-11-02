using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WaterSurfaceCollisionHandler : MonoBehaviour
{
    [SerializeField]
    public Vector3 playerRespawnPosition;
    private void OnCollisionEnter(Collision other) 
    {
        
        if (other.gameObject.tag == "Player")
        {
            foreach(PlayerBinder playerBinder in FindObjectsOfType<PlayerBinder>())
            {
                if (playerBinder.IsBound())
                {
                    playerBinder.Unbind();
                }
            }

            PlayerMovement FPController = other.gameObject.GetComponent<PlayerMovement>();
            FPController.gameObject.GetComponentInParent<PlayerRespawnHandler>().RespawnPlayer();
        }
    }
}
