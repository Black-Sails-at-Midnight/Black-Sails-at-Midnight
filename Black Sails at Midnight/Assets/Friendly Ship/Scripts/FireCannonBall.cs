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
    int NumberOfCannonBalls = 1;
    [SerializeField]
    float Spread = 0f;
    [SerializeField]
    List<GameObject> RightCannons;
    [SerializeField]
    List<GameObject> LeftCannons;
    [SerializeField]
    string TagToFireUpon = "Enemy";

    FireCannonFX cannonFX;

    [SerializeField]
    Transform target;
   

    bool cannonsReady = true;
    void Start()
    {
        cannonFX = gameObject.GetComponentInChildren<FireCannonFX>();
    }

    void Update()
    {
        if(target != null && cannonsReady && true)
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
            for (int i = 0; i < NumberOfCannonBalls; i++)
            {
                float timeDelay = Random.Range(0, 0.1f * TimeBetweenShots);
                timeSpent += timeDelay;

                if (target.IsDestroyed())
                {
                    continue;
                }

                Vector3 randomVector =
                                Quaternion.AngleAxis(Random.Range(-Spread, Spread), Vector3.Cross((target.position - item.transform.position).normalized, Vector3.up)) * (target.position - item.transform.position).normalized +
                                Quaternion.AngleAxis(Random.Range(-Spread, Spread), Vector3.Cross((target.position - item.transform.position).normalized, Vector3.right)) * (target.position - item.transform.position).normalized;

                GameObject instance = Instantiate(CannonBallType, item.transform.position, item.transform.rotation);
                instance.GetComponent<Rigidbody>().AddForce(randomVector.normalized * CannonBallSpeed, ForceMode.Impulse);
                Destroy(instance, 8f);
                yield return new WaitForSeconds(timeDelay);
            }
        }
        yield return new WaitForSeconds(TimeBetweenShots - timeSpent);
        cannonsReady = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagToFireUpon)
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (target != null && other.gameObject.transform == target.transform)
        {
            target = null; // Clear Target;
        }
    }
}
