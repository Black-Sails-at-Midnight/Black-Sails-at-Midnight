using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IslandHealth : MonoBehaviour
{
    public Canvas playerCanvas;
    public Canvas deathCanvas;
    public TextMeshProUGUI gameOverText;

    // Start is called before the first frame update
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

    private float currentHealth;

    // Monobehaviour Methods
    public void Start()
    {
        Health = maxHealth;
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
        if (Health < 0)
        {
            playerCanvas.gameObject.SetActive(false);
            deathCanvas.gameObject.SetActive(true);
            gameOverText.gameObject.SetActive(true);
        }
    }

}
