using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpecialEnemyNavigation : MonoBehaviour
{
    [Serializable]
    enum Direction
    {
        ClockWise = 0,
        Counter_Clockwise = 1
    }
    [Header("Ring Data")]
    [SerializeField]
    int RingNumber = 0;
    [SerializeField]
    RingSystem Ring;
    [SerializeField]
    public int CurrentPosition = 0;
    int NumberOfCoordinates = 0;

    [Header("Navigation Data")]
    [SerializeField]
    Vector3 destination;
    [SerializeField]
    float distanceToTarget = 10f;
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    Direction direction;
    
    [SerializeField]
    bool start = false;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Ring == null)
        {
            if (start)
            {
                FindTarget();
            }
            return;
        }

        switch (direction)
        {
            case Direction.ClockWise:
                ClockWise();
                break;
            case Direction.Counter_Clockwise:
                CounterClockWise();
                break;
            default:
                Debug.Log("Invalid value in direction!");
                break;
        }
    }

    public void StartNavigation()
    {
        start = true;
    }

    private void ClockWise()
    {
        if (Vector3.Distance(agent.transform.position, destination) < distanceToTarget)
        {
            if (NumberOfCoordinates <= CurrentPosition)
            {
                CurrentPosition = 0;
            }
            else
            {
                CurrentPosition++;
            }

            destination = Ring.GetNextPosition(CurrentPosition);
            agent.SetDestination(destination);
        }
    }

    private void CounterClockWise()
    {
        if (Vector3.Distance(agent.transform.position, destination) < distanceToTarget)
        {
            if (NumberOfCoordinates >= 0)
            {
                CurrentPosition = NumberOfCoordinates;
            }
            else
            {
                CurrentPosition--;
            }

            destination = Ring.GetNextPosition(CurrentPosition);
            
            agent.SetDestination(destination);
        }
    }

    public void FindTarget()
    {
        Ring = GameObject.Find("Rings").GetComponent<RingsManager>().GetRing(RingNumber);
        
        destination = Ring.GetNextPosition(CurrentPosition);
        agent.destination = destination;
    }
}
