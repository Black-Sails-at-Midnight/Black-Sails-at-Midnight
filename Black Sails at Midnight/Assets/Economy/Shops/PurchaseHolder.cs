using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseHolder : MonoBehaviour
{
    public GameObject shipToSpawn;

    public int RingNumber;

    [SerializeField]
    Transform spawnPoint;

    public void SpawnShip()
    {
        GameObject ship = Instantiate(shipToSpawn, spawnPoint);

        ShipNavigationAI shipNav = ship.GetComponent<ShipNavigationAI>();

        shipNav.RingNumber = RingNumber;
        shipNav.StartNavigation();
    }
}
