using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IAttackable
{
    [SerializeField]
    public float maxHealth = 100;
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
    private PlayerDeathHandler deathHandler;


    // Monobehaviour Methods
    public void Start()
    {
        Health = maxHealth;
    }


    // Interface Implementations
    public void Hit(GameObject source, Attack attack)
    {
        Health -= attack.attackSettings.damage * attack.attackSettings.damageMultiplier;
    }

    public void Hit(float damage)
    {
        Health -= damage;
    }

    // Public Methods
    public void Heal(GameObject source, float amount)
    {
        if ((Health + amount) > maxHealth)
            Health = maxHealth;
        else
            Health += amount;
    }


    // Private Methods
    private void HealthUpdate()
    {
        if (Health <= 0)
        {
            deathHandler.Die(this.gameObject);
        }
    }
}
