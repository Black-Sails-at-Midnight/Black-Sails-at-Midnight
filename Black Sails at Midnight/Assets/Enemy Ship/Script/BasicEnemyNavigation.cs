using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets._2D;

public class BasicEnemyNavigation : MonoBehaviour
{
    [Header("Ring Data")]
    [SerializeField]
    int RingNumber = 0;
    [SerializeField]
    RingSystem Ring;
    int NumberOfCoordinates = 0;

    [Header("Navigation Data")]
    [SerializeField]
    Vector3 destination;
    [SerializeField]
    NavMeshAgent agent;

    [SerializeField]
    bool start = false;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Ring == null && start)
        {
            StartCoroutine(FindTarget());
        }
    }

    public void StartNavigation()
    {
        start = true;
    }

    public IEnumerator FindTarget()
    {
        Ring = GameObject.Find("Rings").GetComponent<RingsManager>().GetRing(RingNumber);
        /*NumberOfCoordinates = Ring.GetNumberOfCoordinates() - 1;

        Vector3 target = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < NumberOfCoordinates; i++)
        {
            float distance = Vector3.Distance(gameObject.transform.position, Ring.GetNextPosition(i));
            if (target == Vector3.zero || distance < closestDistance)
            {
                closestDistance = distance;
                target = Ring.GetNextPosition(i);
            }
        }

        agent.destination = target;*/
        yield return new WaitForEndOfFrame();
    }
}
