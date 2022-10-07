using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackAttributes : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    float Damage;

    bool isDone = true;
    public virtual void Effect(ShipHealth health)
    {
        return;
    }

    public virtual bool IsEffectDone()
    {
        return isDone;
    }
}
