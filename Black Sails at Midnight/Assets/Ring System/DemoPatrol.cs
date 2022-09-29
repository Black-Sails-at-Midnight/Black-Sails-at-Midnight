using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DemoPatrol : MonoBehaviour
{

    [SerializeField]
    int RingNumber = 0;
    [SerializeField]
    RingSystem Ring;

    [SerializeField]
    int CurrentPosition = 0;

    int NumberOfCoordinates = 0;

    [SerializeField]
    Vector3 destination;

    [SerializeField]
    NavMeshAgent agent;

    bool isCheckingForRing = false;

    void Update()
    {
        if(Ring == null && isCheckingForRing == false)
        {
            StartCoroutine(GetRing());
            return;
        }


        if(Vector3.Distance(agent.transform.position,destination) < 1f)
        {
            if(NumberOfCoordinates <= CurrentPosition)
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

    private IEnumerator GetRing()
    {
        isCheckingForRing = true;
        Ring = GameObject.Find("Rings").GetComponent<RingsManager>().GetRing(RingNumber);
        NumberOfCoordinates = Ring.GetNumberOfCoordinates();
        if (Ring != null)
        {
            agent.destination = Ring.GetNextPosition(CurrentPosition);
        }
        yield return new WaitForSeconds(1);
        isCheckingForRing = false;
    }
}
