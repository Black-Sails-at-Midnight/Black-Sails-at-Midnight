using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseShip : PurchaseOption, InteractableCanvasObject
{
    [SerializeField]
    GameObject prefab;

    [SerializeField]
    GameObject RingSelectionCanvas;

    public void CanvasAction()
    {
        Purchase();
    }

    public override void Purchase()
    {
        GetComponentInParent<PurchaseHolder>().shipToSpawn = prefab;
        RingSelectionCanvas.SetActive(true);
        GameObject.Find("Ship Shop Canvas").SetActive(false);
    }
}
