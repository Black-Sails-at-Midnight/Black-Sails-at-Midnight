using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipNavigationAI : MonoBehaviour
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
    int CurrentPosition = 0;
    int NumberOfCoordinates = 0;

    [Header("Navigation Data")]
    [SerializeField]
    Vector3 destination;
    [SerializeField]
    float distanceToDestination = 15f;
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    Direction direction;
    private bool isCheckingForRing = false;

    void Update()
    {
        if (Ring == null && !isCheckingForRing)
        {
            StartCoroutine(GetRing());
            return;
        }
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

    private IEnumerator GetRing()
    {
        isCheckingForRing = true;
        Ring = GameObject.Find("Rings").GetComponent<RingsManager>().GetRing(RingNumber);
        NumberOfCoordinates = Ring.GetNumberOfCoordinates() - 1;
        if (Ring != null)
        {
            agent.destination = Ring.GetNextPosition(CurrentPosition);
            if (RingNumber % 2 == 0)
            {
                direction = Direction.ClockWise;
            }
            else
            {
                direction = Direction.Counter_Clockwise;
            }
        }
        yield return new WaitForSeconds(1);
        isCheckingForRing = false;
    }

    private void ClockWise()
    {
        if (Vector3.Distance(agent.transform.position, agent.destination) < distanceToDestination)
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
        if (Vector3.Distance(agent.transform.position, agent.destination) < distanceToDestination)
        {
            if (NumberOfCoordinates <= 0)
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
}
