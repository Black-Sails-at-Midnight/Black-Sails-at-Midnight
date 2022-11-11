using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FireCannonSingle : MonoBehaviour
{
    [SerializeField]
    GameObject CannonBallType;
    [SerializeField]
    float TimeBetweenShots;
    [SerializeField]
    float CannonBallSpeed = 100;
    [SerializeField]
    float FiringAngle = 30;
    [SerializeField]
    GameObject Cannon;
    [SerializeField]
    string TagToFireUpon = "Enemy";
    [SerializeField]
    string FinalTarget = "Island";
    [SerializeField]
    private bool isHostile = false;
    [SerializeField]
    FireCannonFX cannonFX;
    private List<Transform> shipsInRange;

    Transform target;
   
    bool cannonsReady = true;

    void Start()
    {
        cannonFX = gameObject.GetComponentInChildren<FireCannonFX>();
        shipsInRange = new List<Transform>();
    }

    void Update()
    {
        if(target != null && cannonsReady && IsWithinFiringArc())
        {
            Vector3 Direction = transform.InverseTransformPoint(target.position);
            if (Direction.z > 0) // Target is in front
            {
                StartCoroutine(FireCannon());
            }
        }
    }

    private bool IsWithinFiringArc()
    {
        float z = transform.InverseTransformDirection(target.position).z;

        if (z <= FiringAngle && z >= -FiringAngle)
        {
            return true;
        }
            return false;
    }

    IEnumerator FireCannon()
    {
        cannonsReady = false;
        GameObject instance = Instantiate(CannonBallType, Cannon.transform.position, Cannon.transform.rotation);
        instance.GetComponent<Rigidbody>().AddForce((target.position - Cannon.transform.position).normalized * CannonBallSpeed, ForceMode.Impulse);
        Destroy(instance, 8f);
        cannonFX.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(TimeBetweenShots);
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
