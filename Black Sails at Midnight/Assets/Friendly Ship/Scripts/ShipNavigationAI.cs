using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipNavigationAI : MonoBehaviour
{
    [Serializable]
    public enum Direction
    {
        ClockWise = 0,
        Counter_Clockwise = 1
    }
    [Header("Ring Data")]
    [SerializeField]
    public int RingNumber = 0;
    [SerializeField]
    RingSystem Ring;
    [SerializeField]
    public int CurrentPosition = 0;

    [Header("Navigation Data")]
    [SerializeField]
    public Vector3 destination;
    [SerializeField]
    public float distanceToDestination = 15f;
    [SerializeField]
    public NavMeshAgent agent;
    [SerializeField]
    public Direction direction;
    private bool isCheckingForRing = false;

    float baseSpeed;

    private void Start()
    {
        baseSpeed = agent.speed;
    }

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

        if (Vector3.Distance(agent.transform.position, agent.destination) < distanceToDestination)
        {
            ShipReachedDestination();
        }
    }

    private IEnumerator GetRing()
    {
        isCheckingForRing = true;
        Ring = GameObject.Find("Rings").GetComponent<RingsManager>().GetRing(RingNumber);
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

    public void SetDestination(Vector3 destination)
    {
        agent.destination = destination;
    }

    public void IncreaseAgentSpeed()
    {
        agent.speed *= 1.5f;
    }    

    public void ResetAgentSpeed()
    {
        agent.speed = baseSpeed;
    }

    void ShipReachedDestination()
    {
        if(!Ring.CheckIfWaitingForUpdate(this))
        {
            Ring.TriggerDestinationUpdate(this);
        }
    }
}
