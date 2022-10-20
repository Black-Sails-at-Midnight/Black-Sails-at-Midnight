using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSynchronisationInRing : MonoBehaviour, InteractableCanvasObject
{
    [SerializeField]
    int RingNumber;
    private void Start()
    {
        Int32.TryParse(name.Split(' ')[1], out RingNumber);
    }

    public void CanvasAction()
    {
        FindObjectOfType<RingsManager>().GetRing(RingNumber).ForceSync();
    }
}
