using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChainShot : CannonBallProperties
{
    public override void Effect(ShipHealth health)
    {
        health.EnableStatusEffect(StatusEffectType.SlowDown);
    }
}
