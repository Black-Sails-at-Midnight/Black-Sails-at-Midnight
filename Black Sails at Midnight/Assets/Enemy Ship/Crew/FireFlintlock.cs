using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlintlock : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    float fireDelay = 2;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    [Range(0, 10)]
    float bulletSpeed = 2;

    [SerializeField]
    Transform SpawnPosition;

    Transform target;

    bool readyToShoot = true;

    private void Start()
    {
        target = GameObject.Find("FirstPersonCharacter").transform;
    }

    private void Update()
    {
        if (readyToShoot)
        {
            StartCoroutine(FireWeapon());
        }
    }

    private IEnumerator FireWeapon()
    {
        readyToShoot = false;
        GameObject instance = Instantiate(bulletPrefab, SpawnPosition.position, SpawnPosition.rotation);
        instance.GetComponent<Rigidbody>().AddForce((target.position - SpawnPosition.position).normalized * bulletSpeed, ForceMode.Impulse);
        Destroy(instance, 8f);
        yield return new WaitForSeconds(fireDelay);
        readyToShoot = true;
    }
}
