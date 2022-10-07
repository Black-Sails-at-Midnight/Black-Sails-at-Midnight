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
        throw new System.NotImplementedException();
    }
}
