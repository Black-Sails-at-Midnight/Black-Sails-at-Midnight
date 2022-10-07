using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CannonBallProperties : MonoBehaviour
{
    [Header("Cannon Ball Properties")]
    [SerializeField]
    float Damage;
    bool isDone = true;

    public virtual void Effect(ShipHealth health)
    {
        
    }

    public bool IsEffectDone()
    {
        return isDone;
    }
}



