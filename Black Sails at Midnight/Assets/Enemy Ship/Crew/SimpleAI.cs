using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAI : MonoBehaviour
{

    [SerializeField]
    NavMeshAgent agent;

    [SerializeField]
    Transform target;

    [SerializeField]
    Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            GetComponent<Animator>().SetBool("SpottedPlayer", true);
        }
        agent.destination = target.position;
        destination = agent.destination;
    }

    public void TargetEntity(Transform target)
    {
        this.target = target;
    }    
}
