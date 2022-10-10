using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttack : MonoBehaviour
{
    [SerializeField]
    float damage;

    [SerializeField]
    float Interval = 1f;

    bool isAttacking = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Hello!");
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer(other.gameObject));
            }
        }
    }

    private IEnumerator AttackPlayer(GameObject other)
    {
        isAttacking = true;
        other.GetComponent<PlayerHealth>().Hit(damage);
        yield return new WaitForSeconds(Interval);
        isAttacking = false;
    }
}
