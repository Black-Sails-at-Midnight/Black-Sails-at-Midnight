using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChainShot : CannonBallProperties
{
        [Header("Chainshot Properties")]
        [SerializeField]
        [Range(0,100)]
        float SlowDownPercentage;

    public override void Effect(ShipHealth health)
    {
        ShipNavigationAI AI = health.gameObject.GetComponent<ShipNavigationAI>();
        // Not the neatest, but this shouldn't be done more than once.
        if (AI.baseSpeed == AI.baseSpeed * SlowDownPercentage / 100)
        {
            AI.SetAgentSpeed(SlowDownPercentage / 100);
        }
    }
    public override bool IsEffectDone()
    {
        return base.IsEffectDone();
    }

}
