using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private float damage = 20;
    private void OnTriggerEnter(Collider other) //In an ideal world, where the deadline could be moved, I'd write better code. Sadly, this isn't an ideal world.
    {
        if (other.tag == "Player")
        {
            //other.GetComponent<PlayerHealth>().Hit(damage);
            Destroy(gameObject);
        }
        else if (other.tag != "EnemyCrew" && other.tag != "Enemy" && other.tag != "Projectile")
        {
            Destroy(gameObject);
        }
    }
}
