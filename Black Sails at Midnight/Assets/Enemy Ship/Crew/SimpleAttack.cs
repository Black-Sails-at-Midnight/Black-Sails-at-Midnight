using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttack : MonoBehaviour
{
    [SerializeField]
    float damage = 10;

    [SerializeField]
    float Interval = 1f;

    bool isAttacking = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer(other.gameObject));
            }
        }
    }

    private IEnumerator AttackPlayer(GameObject other)
    {
        isAttacking = true;
        other.GetComponentInParent<PlayerHealth>().Hit(damage);
        yield return new WaitForSeconds(Interval);
        isAttacking = false;
    }
}
