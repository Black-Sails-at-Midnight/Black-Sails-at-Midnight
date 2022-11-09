using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerDeathHandler : MonoBehaviour
{
    public void Die(GameObject player)
    {
        player.GetComponent<PlayerRespawnHandler>().RespawnPlayer();
        player.GetComponent<PlayerHealth>().Heal(gameObject,player.GetComponent<PlayerHealth>().maxHealth);
    }
}
