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
    GameObject cannonFX;

    Transform target;
   

    bool cannonsReady = true;
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
        Debug.Log("FIRING");
        GameObject instance = Instantiate(CannonBallType, Cannon.transform.position, Cannon.transform.rotation);
        instance.GetComponent<Rigidbody>().AddForce((target.position - Cannon.transform.position).normalized * CannonBallSpeed, ForceMode.Impulse);
        Destroy(instance, 8f);
        cannonFX.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(TimeBetweenShots);
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
