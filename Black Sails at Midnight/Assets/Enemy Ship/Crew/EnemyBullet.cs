using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private float damage = 20;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().Hit(damage);
        }
        else if (other.tag != "Crew")
        {
            Destroy(gameObject);
        }
    }
}
