using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSelector : MonoBehaviour, InteractableCanvasObject
{
    [SerializeField]
    int RingNumber;

    [SerializeField]
    GameObject ShipShopCanvas;

    private void Start()
    {
        Int32.TryParse(name.Split(' ')[1], out RingNumber);
    }

    public void CanvasAction()
    {
        GetComponentInParent<PurchaseHolder>().RingNumber = RingNumber;
        GetComponentInParent<PurchaseHolder>().SpawnShip();
        ShipShopCanvas.SetActive(true);
        GameObject.Find("Ring Selection Canvas").SetActive(false);
    }

}
