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
    float FiringAngle = 30;
    [SerializeField]
    List<GameObject> RightCannons;
    [SerializeField]
    List<GameObject> LeftCannons;

    FireCannonFX cannonFX;

    Transform target;
   

    bool cannonsReady = true;
    void Start()
    {
        cannonFX = gameObject.GetComponentInChildren<FireCannonFX>();
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
            IsWithinFiringArc();
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

    IEnumerator FireCannons(List<GameObject> cannons)
    {
        cannonsReady = false;
        float timeSpent = 0;
        foreach (GameObject item in cannons)
        {
            float timeDelay = Random.Range(0, 0.1f * TimeBetweenShots);
            timeSpent += timeDelay;
            GameObject instance = Instantiate(CannonBallType, item.transform.position, item.transform.rotation);
            instance.GetComponent<Rigidbody>().AddForce((target.position - item.transform.position).normalized * CannonBallSpeed, ForceMode.Impulse);
            Destroy(instance, 8f);
            yield return new WaitForSeconds(timeDelay);
        }
        yield return new WaitForSeconds(TimeBetweenShots - timeSpent);
        cannonsReady = true;
    }

    // NEEDS WORK

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.tag == "Player")
        {
            target = null;
        }
    }
}