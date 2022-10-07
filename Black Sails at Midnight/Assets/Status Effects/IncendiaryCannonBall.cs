using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncendiaryCannonBall : CannonBallProperties
{
        [Header("Incendiary Properties")]
        [SerializeField]
        float DamageOverTime;
        [SerializeField]
        float ChanceToExtinguish;

    public override void Effect(ShipHealth health)
    {
        health.Hit(DamageOverTime);
    }

    public override bool IsEffectDone()
    {
        if (UnityEngine.Random.Range(0f, 1f) <= ChanceToExtinguish)
        {
            return true;
        }
        return false;
    }
}
