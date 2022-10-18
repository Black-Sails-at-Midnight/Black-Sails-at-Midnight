using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncendiaryCannonBall : CannonBallProperties
{
    public override void Effect(ShipHealth health)
    {
        health.EnableStatusEffect(StatusEffectType.Incendiary);
    }
}
