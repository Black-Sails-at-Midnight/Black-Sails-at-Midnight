using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CannonBallProperties : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    protected float Damage;

    bool isDone = true;
    public virtual void Effect(ShipHealth health)
    {
        return;
    }

    public virtual bool IsEffectDone()
    {
        return isDone;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<ShipHealth>().Hit(Damage);
        }
    }
}
