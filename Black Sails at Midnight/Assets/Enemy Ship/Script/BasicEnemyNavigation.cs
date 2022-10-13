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

    [SerializeField]
    public int targetCoordinate;

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
            FindTarget();
        }
    }

    public void StartNavigation()
    {
        start = true;
    }

    public void FindTarget()
    {
        Ring = GameObject.Find("Rings").GetComponent<RingsManager>().GetRing(RingNumber);
        
        destination = Ring.GetNextPosition(targetCoordinate);
        agent.destination = destination;
    }
}
