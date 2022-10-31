using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetonateShip : MonoBehaviour, InteractableCanvasObject
{
    [SerializeField]
    float damage = 100f;

    [SerializeField]
    GameObject parent;

    [SerializeField]
    int Delay = 3;

    bool hasIgnited = false;
    public void CanvasAction()
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        if (!hasIgnited)
        {
            hasIgnited = true;
            RespawnPlayer();
            yield return new WaitForSeconds(Delay);
            List<ShipHealth> enemies = parent.GetComponentInChildren<FireCannonBall>().GetAllShipsInRange();

            foreach (var item in enemies)
            {
                item.Hit(damage);
            }

            //Animation here :)
            Destroy(parent);
        }
    }

    private void RespawnPlayer()
    {
        PlayerBinder playerBinder = GetComponentsInParent<PlayerBinder>()[0];
        playerBinder.Unbind();

        FindObjectOfType<PlayerRespawnHandler>().RespawnPlayer();
    }
}
