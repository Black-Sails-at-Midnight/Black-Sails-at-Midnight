using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public float maxHealth = 100;
    [SerializeField]
    List<CannonBallProperties> StatusEffects;

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

    [SerializeField]
    float currentHealth;

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


    // Private Methods
    private void HealthUpdate()
    {
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("BELOW 0 MOTHERFUCKER | " + gameObject.name);
            FindObjectOfType<EconomySystem>().Deposit(gameObject.GetComponent<BasicShipEquivelant>().GetBasicEquivelant());
            Destroy(gameObject);            //TODO: have an animation or something, but it's FINE for now.
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


    public void AddStatusEffect(CannonBallProperties effect)
    {
        if (effect is CannonBallProperties)
        {
            return;
        }
        else
        {
            StatusEffects.Add(effect); // Might not work as desired, due to obj destruction
        }
    }
}
