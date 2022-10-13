using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PurchaseOption : MonoBehaviour, IPurchase
{
    [SerializeField]
    int cost;

    public int Cost { get { return cost; } }
    public abstract void Purchase();
}

public interface IPurchase
{
    public void Purchase();
}
