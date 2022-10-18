using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthHandler : MonoBehaviour
{
    [SerializeField]
    public float health;
    [SerializeField]
    public float maxHealth;
    [SerializeField]
    public float healthPercentage;
    private Slider slider;
    private PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        health = playerHealth.Health;
        maxHealth = playerHealth.maxHealth;
        healthPercentage = (playerHealth.Health / playerHealth.maxHealth);

        slider.value = (playerHealth.Health / playerHealth.maxHealth);
    }
}
