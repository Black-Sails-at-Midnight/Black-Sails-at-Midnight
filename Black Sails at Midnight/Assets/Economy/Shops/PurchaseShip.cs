using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseShip : PurchaseOption
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    Transform spawnPoint;

    public override bool Purchase()
    {
        GameObject ship = Instantiate(prefab, spawnPoint);

        ShipNavigationAI shipNav = ship.GetComponent<ShipNavigationAI>();

        shipNav.RingNumber = 3;
        shipNav.StartNavigation();

        Debug.Log("Ship Spawned");
        return true;
    }
}
