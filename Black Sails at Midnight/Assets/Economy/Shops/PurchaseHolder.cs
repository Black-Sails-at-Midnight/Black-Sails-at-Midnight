using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PurchaseHolder : MonoBehaviour
{
    public GameObject shipToSpawn;
    public int RingNumber;


    [SerializeField]
    int ShipCost;

    [SerializeField]
    Transform spawnPoint;

    private EconomySystem PlayerWallet;
    private RingsManager ringsManager;
    private void Start()
    {
        PlayerWallet = FindObjectOfType<EconomySystem>();
        ringsManager = FindObjectOfType<RingsManager>();
    }

    public void SpawnShip()
    {
        if (!ringsManager.GetRing(RingNumber).isFull())
        {
            GameObject ship = Instantiate(shipToSpawn, spawnPoint);

            ShipNavigationAI shipNav = ship.GetComponent<ShipNavigationAI>();

            shipNav.RingNumber = RingNumber;

            shipNav.StartNavigation();

            PlayerWallet.Withdraw(ShipCost);
        }
    }

    public void SetShipCost(int value)
    {
        ShipCost = value;
    }
}
