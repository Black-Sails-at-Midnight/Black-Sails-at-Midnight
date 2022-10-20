using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ApproachSpeedHandler : MonoBehaviour
{
    [SerializeField]
    float approachSpeedMultiplier = 3f;

    
    private RingSystem lastRingWithouthShips;

    void Start()
    {
        GetComponent<NavMeshAgent>().speed *= approachSpeedMultiplier;
        StartCoroutine(FindLastRingWithoutShips());
    }

    void Update()
    {
        float distanceToIsland = Vector3.Distance(Vector3.zero, transform.position);

        if (distanceToIsland < lastRingWithouthShips.Radius)
        {
            ArrivedAtDestination();
        }
    }

    private void ArrivedAtDestination()
    {
        GetComponent<NavMeshAgent>().speed /= approachSpeedMultiplier;
        StopAllCoroutines();
        this.enabled = false;
    }

    private IEnumerator FindLastRingWithoutShips()
    {
        while (true)
        {
            RingsManager ringsManager = FindObjectOfType<RingsManager>();

            int numberOfRings = ringsManager.GetNumberOfRings();
            int latestRingWithShips = 0;

            for (int i = 0; i < numberOfRings; i++)
            {
                if (ringsManager.GetRing(i).DisableMarker == true)
                    continue;

                if (ringsManager.GetRing(i).Ships.Count > 0)
                {
                    latestRingWithShips = i;
                }
            }

            lastRingWithouthShips = ringsManager.GetRing(latestRingWithShips + 1);
            yield return new WaitForSeconds(1);
        }
    }
}
