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

    public override void Effect()
    {
        
    }
}
