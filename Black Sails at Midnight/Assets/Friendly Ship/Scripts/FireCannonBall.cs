using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FireCannonBall : MonoBehaviour
{
    [SerializeField]
    GameObject CannonBallType;
    [SerializeField]
    float TimeBetweenShots;
    [SerializeField]
    float CannonBallSpeed = 100;
    [SerializeField]
    float FiringAngle = 90f;
    [SerializeField]
    int NumberOfCannonBalls = 1;
    [SerializeField]
    float Spread = 0f;
    [SerializeField]
    List<GameObject> RightCannons;
    [SerializeField]
    List<GameObject> LeftCannons;
    [SerializeField]
    string TagToFireUpon = "Enemy";
    [SerializeField]
    string FinalTarget = "Island";
    [SerializeField]
    private bool isHostile = false;
    
    [SerializeField]
    float range = 100f;

    FireCannonFX cannonFX;

    [SerializeField]
    Transform target;
   
    private List<Transform> shipsInRange;
    bool cannonsReady = true;

    void Start()
    {
        cannonFX = gameObject.GetComponentInChildren<FireCannonFX>();
        shipsInRange = new List<Transform>();

        if (GetComponent<SphereCollider>() != null)
        {
            GetComponent<SphereCollider>().radius = range;            
        }

        if (GetComponentInChildren<RingMarkerHandler>() != null)
        {
            GetComponentInChildren<RingMarkerHandler>().Radius = range;
        }
    }

    void Update()
    {
        if(target != null && cannonsReady && IsWithinFiringArc())
        {
            Vector3 Direction = transform.InverseTransformPoint(target.position);
            if (Direction.x < 0) // Target is Left
            {
                cannonFX.FireRightCannons();
                StartCoroutine(FireCannons(RightCannons));
            }
            else if(Direction.x > 0) // Target is Right
            {
                cannonFX.FireLeftCannons();
                StartCoroutine(FireCannons(LeftCannons));
            }
        }
    }

    private bool IsWithinFiringArc()
    {
        Vector3 vectorToTarget = (target.transform.position - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, vectorToTarget);

        return ((angleToTarget >= 90 - (FiringAngle / 2)) && (angleToTarget <= 90 + (FiringAngle / 2))) || ((angleToTarget >= 270 - (FiringAngle / 2)) && (angleToTarget <= 270 + (FiringAngle / 2)));
    }

    IEnumerator FireCannons(List<GameObject> cannons)
    {
        cannonsReady = false;
        float timeSpent = 0;
        foreach (GameObject item in cannons)
        {
            for (int i = 0; i < NumberOfCannonBalls; i++)
            {
                if (target.IsDestroyed() || target == null)
                {
                    continue;
                }

                Vector3 randomVector =
                                Quaternion.AngleAxis(Random.Range(-Spread, Spread), Vector3.Cross((target.position - item.transform.position).normalized, Vector3.up)) * (target.position - item.transform.position).normalized +
                                Quaternion.AngleAxis(Random.Range(-Spread, Spread), Vector3.Cross((target.position - item.transform.position).normalized, Vector3.right)) * (target.position - item.transform.position).normalized;

                GameObject instance = Instantiate(CannonBallType, item.transform.position, item.transform.rotation);
                instance.GetComponent<Rigidbody>().AddForce(randomVector.normalized * CannonBallSpeed, ForceMode.Impulse);
                Destroy(instance, 8f);
            }
    
            float timeDelay = Random.Range(0, 0.1f * TimeBetweenShots);
            timeSpent += timeDelay;

            yield return new WaitForSeconds(timeDelay);
        }
        yield return new WaitForSeconds(TimeBetweenShots - timeSpent);
        cannonsReady = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == FinalTarget && isHostile)
        {
            shipsInRange.Add(other.transform);

        }
        else if (other.tag == TagToFireUpon && other is MeshCollider)
        {
            shipsInRange.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        shipsInRange.RemoveAll(x => x == null || x.IsDestroyed());

        if (other.tag == TagToFireUpon && shipsInRange.Contains(other.transform))
        {
            shipsInRange.Remove(other.transform);
        }

        if (target == other.transform)
        {
            target = null;
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        shipsInRange.RemoveAll(x => x == null || x.IsDestroyed());
        shipsInRange.Sort(SortOnDistanceToIsland);
        
        if (shipsInRange.Count > 0)
        {
            target = shipsInRange[0];
        }
    }

    private int SortOnDistanceToIsland(Transform a, Transform b)
    {
        Vector3 islandPosition = Vector3.zero;

        float distanceA = Vector3.Distance(a.position, islandPosition);
        float distanceB = Vector3.Distance(b.position, islandPosition);

        if (distanceA > distanceB)
            return 1;

        if (distanceB > distanceA)
            return -1;
        
        return 0;
    }
    

    public List<ShipHealth> GetAllShipsInRange()
    {
        List<ShipHealth> enemyShips = new List<ShipHealth>();
        foreach (var item in shipsInRange)
        {
            enemyShips.Add(item.gameObject.GetComponentInChildren<ShipHealth>());
        }
        return enemyShips;
    }
}
