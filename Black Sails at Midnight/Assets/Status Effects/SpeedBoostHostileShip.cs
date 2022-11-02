using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpeedBoostHostileShip : MonoBehaviour
{
    [SerializeField]
    float SpeedMultiplier;

    [SerializeField]
    float TimeDelay;

    [SerializeField]
    List<GameObject> ShipsInRange;

    [SerializeField]
    float BaseSpeed;

    bool isBuffing = false;

    private void Start()
    {
        BaseSpeed = this.GetComponent<NavMeshAgent>().speed;
    }

    private void Update()
    {
        if (!isBuffing)
        {
            StartCoroutine(GiveSpeedBoost());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            ShipsInRange.Add(other.gameObject);
        }
    }

    IEnumerator GiveSpeedBoost()
    {
        isBuffing = true;
        foreach (var item in ShipsInRange)
        {
            item.GetComponent<NavMeshAgent>().speed = BaseSpeed * SpeedMultiplier;
        }
        yield return new WaitForSeconds(TimeDelay);
        isBuffing = false;
    }
}
