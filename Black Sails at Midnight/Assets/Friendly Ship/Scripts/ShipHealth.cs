using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public float maxHealth = 100;
    [SerializeField]
    List<AttackAttributes> StatusEffects;

    bool isCheckingStatusEffects = false;

    public float Health
    {
        get { return currentHealth; }
        private set
        {
            currentHealth = value;
            HealthUpdate();
        }
    }

    private float currentHealth;

    // Monobehaviour Methods
    public void Start()
    {
        Health = maxHealth;
    }

    public void Update()
    {
        if (!isCheckingStatusEffects)
        {
            StartCoroutine(ApplyStatusEffects());
        }
    }

    public void Hit(Attack attack)
    {
        Health = -attack.attackSettings.damage * attack.attackSettings.damageMultiplier;
    }

    public void Hit(float damage)
    {
        Health = -damage;
    }

    public void Heal(float amount)
    {
        if ((Health + amount) > maxHealth)
            Health = maxHealth;
        else
            Health += amount;
    }


    // Private Methods
    private void HealthUpdate()
    {
        if (Health < 0)
        {
            //TODO: Remove Enemy from play.
            Destroy(gameObject);
        }
    }

    IEnumerator ApplyStatusEffects()
    {
        isCheckingStatusEffects = true;
        foreach (var item in StatusEffects)
        {
            if (!item.IsEffectDone())
            {
                item.Effect(this);
            }
            else
            {
                StatusEffects.Remove(item);
            }
        }
        yield return new WaitForSeconds(1);
        isCheckingStatusEffects = false;
    }


    public void AddStatusEffect(AttackAttributes effect)
    {
        if (effect is AttackAttributes)
        {
            return;
        }
        else
        {
            StatusEffects.Add(effect); // Might not work as desired, due to obj destruction
        }
    }
}
