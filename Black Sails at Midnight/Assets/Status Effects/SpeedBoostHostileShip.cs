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

    bool isRegenerating = false;

    private void Start()
    {
        BaseSpeed = this.GetComponent<NavMeshAgent>().speed;
    }

    private void Update()
    {
        if (!isRegenerating)
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

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            ShipsInRange.Remove(other.gameObject);
        }
    }

    IEnumerator GiveSpeedBoost()
    {
        isRegenerating = true;
        foreach (var item in ShipsInRange)
        {
            item.GetComponent<NavMeshAgent>().speed = BaseSpeed * SpeedMultiplier;
        }
        yield return new WaitForSeconds(TimeDelay);
        isRegenerating = false;
    }
}
