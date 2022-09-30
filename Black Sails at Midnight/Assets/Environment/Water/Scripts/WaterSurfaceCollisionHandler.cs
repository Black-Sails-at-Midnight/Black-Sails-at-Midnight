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
            foreach(PlayerBinder playerBinder in FindObjectsOfType<PlayerBinder>())
            {
                if (playerBinder.parentedPlayer != null)
                {
                    playerBinder.Unbind();
                }
            }

            FirstPersonController FPController = other.gameObject.GetComponent<FirstPersonController>();
            FPController.gameObject.GetComponentInParent<PlayerRespawnHandler>().RespawnPlayer();
        }
    }
}
