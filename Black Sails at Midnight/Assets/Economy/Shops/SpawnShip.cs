using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShip : MonoBehaviour
{

    [SerializeField]
    GameObject Prefab; // Change to Ship

    [SerializeField]
    Transform SpawnPoint;

    private void Start()
    {
        
    }

    public bool PurchaseShip()
    {
        throw new System.NotImplementedException();
    }

    public int GetPriceOfShip()
    {
        throw new System.NotImplementedException();
    }
}
