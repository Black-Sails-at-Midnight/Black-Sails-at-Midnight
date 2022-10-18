using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

public enum StatusEffectType
{
    Incendiary,
    SlowDown,
    SpeedBoost,
    Regeneration
}

[Serializable]
public class StatusEffectTracker
{
    public StatusEffectType type { get; private set; }
    public bool isActive {get; private set;}
    private bool wasActive { get; set; }
    public bool isSingleUse { get; private set; }


    public StatusEffectTracker(StatusEffectType type)
    {
        this.type = type;
        isSingleUse = false;
        isActive = false;
        wasActive = false;
    }

    public StatusEffectTracker(StatusEffectType type, bool isSingleUse) : this(type)
    {
        this.isSingleUse = isSingleUse;
    }

    public void Enable()
    {
        if (wasActive && isSingleUse)
        {
            return;
        }
        wasActive = true;
        isActive = true;
    }

    public void Disable()
    {
        isActive = false;
    }
}

public interface IStatusEffect
{
    [SerializeField]
    public static int chanceToElapse = 10;
    public bool isDone();
}

public interface ISpeedEffect : IStatusEffect
{
    [SerializeField]
    public static float SpeedDifference = 0.3f;
}

// ====================================
//  BAD EFFECTS
// ====================================

public interface IFlammable : IStatusEffect
{
    [SerializeField]
    public static int TickDamage = 10;
    public void FireDamage();
}

public interface ISlowDown : ISpeedEffect
{
    public void SlowDown();
}

// ====================================
//  GOOD EFFECTS
// ====================================

public interface ISpeedBoost : ISpeedEffect
{
    public void SpeedBoost();
}

public interface IRegenerate : IStatusEffect
{
    [SerializeField]
    public static float HealthPerSecond = 10;
    public void Regenerate();
}