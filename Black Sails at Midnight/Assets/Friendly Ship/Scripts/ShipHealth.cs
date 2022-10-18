using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public float maxHealth = 100;

    private bool isApplyingStatusEffects = false;

    public float Health
    {
        get { return currentHealth; }
        private set
        {
            currentHealth = value;
            HealthUpdate();
        }
    }


    [SerializeField]
    float currentHealth;

    // Monobehaviour Methods
    public void Start()
    {
        Health = maxHealth;
        StatusEffects = new List<StatusEffectTracker>();

        bool isSingleUse = true;

        StatusEffects.Add(new StatusEffectTracker(StatusEffectType.Incendiary));
        StatusEffects.Add(new StatusEffectTracker(StatusEffectType.SlowDown, isSingleUse));
        StatusEffects.Add(new StatusEffectTracker(StatusEffectType.SpeedBoost, isSingleUse));
        StatusEffects.Add(new StatusEffectTracker(StatusEffectType.Regeneration));
        
    }

    public void Update()
    {
        if (!isApplyingStatusEffects)
        {
            StartCoroutine(ApplyStatusEffects());
        }
    }

    public void Hit(Attack attack)
    {
        Health -= attack.attackSettings.damage * attack.attackSettings.damageMultiplier;
    }

    public void Hit(float damage)
    {
        Health -= damage;
    }

    public void Heal(float amount)
    {
        if ((Health + amount) > maxHealth)
            Health = maxHealth;
        else
            Health += amount;
    }

    private void HealthUpdate()
    {
        if (currentHealth <= 0)
        {
            FindObjectOfType<EconomySystem>().Deposit(gameObject.GetComponent<BasicShipEquivelant>().GetBasicEquivelant());
            Destroy(gameObject);            //TODO: have an animation or something, but it's FINE for now.
        }
    }

    // ====================================
    //           STATUS EFFECTS
    // ====================================

    List<StatusEffectTracker> StatusEffects;

    IEnumerator ApplyStatusEffects()
    {
        isApplyingStatusEffects = true;

        foreach (var item in StatusEffects)
        {
            if (item.isActive)
            {
                switch (item.type)
                {
                    case StatusEffectType.Incendiary:
                        {
                            FireDamage();
                            if (isDone())
                            {
                                item.Disable();
                            }
                            break;
                        }
                    case StatusEffectType.SlowDown:
                        {
                            SlowDown();
                            item.Disable();
                            break;
                        }
                    case StatusEffectType.SpeedBoost:
                        {
                            SpeedBoost();
                            item.Disable();
                            break;
                        }
                    case StatusEffectType.Regeneration:
                        {
                            Regenerate();
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        yield return new WaitForSeconds(1);
        isApplyingStatusEffects = false;
    }


    public void EnableStatusEffect(StatusEffectType type)
    {
        StatusEffects[(int)type].Enable();
    }

    public void DisableStatusEffect(StatusEffectType type)
    {
        foreach (var item in StatusEffects)
        {
            if (type == item.type)
            {
                item.Disable();
            }
        }
    }

    void FireDamage()
    {
        this.Hit(IFlammable.TickDamage);
    }

    bool isDone()
    {
        if (UnityEngine.Random.Range(0, 100f) <= IStatusEffect.chanceToElapse) { return true; }
        else { return false; }
    }

    void SlowDown()
    {
        GetComponent<NavMeshAgent>().speed *= 1 - ISlowDown.SpeedDifference;
        Debug.Log(GetComponent<NavMeshAgent>().speed);
    }

    void Regenerate()
    {
        this.Heal(IRegenerate.HealthPerSecond);
    }

    void SpeedBoost()
    {
        GetComponent<NavMeshAgent>().speed *= 1 + ISlowDown.SpeedDifference;
    }
}
