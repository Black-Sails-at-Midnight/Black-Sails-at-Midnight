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
    NavMeshAgent agent;
    [SerializeField]
    Direction direction;

    void Start()
    {
        Ring = GameObject.Find("Rings").GetComponent<RingsManager>().GetRing(RingNumber);
        NumberOfCoordinates = Ring.GetNumberOfCoordinates() - 1;

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

        agent.destination = target;

    }

    void Update()
    {
        if (Ring == null)
        {
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

    private void ClockWise()
    {
        if (Vector3.Distance(agent.transform.position, destination) < 1f)
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
        if (Vector3.Distance(agent.transform.position, destination) < 1f)
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

    public void ForceWaypointUpdate()
    {
        ClockWise();
    }
}
