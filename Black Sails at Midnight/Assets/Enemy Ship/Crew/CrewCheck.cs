using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewCheck : MonoBehaviour
{
    private Transform[] Crew;
    private void Start()
    {
        Crew = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Crew[i] = transform.GetChild(i);
        }
    }

    public bool isAllCrewKilled()
    {
        foreach (Transform child in Crew)
        {
            if (child.gameObject.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }

    public void DestroyShip()
    {
        transform.parent.GetComponent<ShipHealth>().Hit(9999); //Well, that should kill it. I hope. 
    }

    public void DisableCrew()
    {
        foreach (Transform child in Crew)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void EnableCrew(Collider other)
    {
        foreach (Transform child in Crew)
        {
            child.gameObject.SetActive(true);
            child.gameObject.GetComponent<SimpleAI>().TargetEntity(other.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnableCrew(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            DisableCrew();
        }
    }
}
