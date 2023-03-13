using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float maxStamina = 100f;

    float currentHealth;
    float currentStamina;

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            // Player is dead, handle death here
        }
    }

    public void UseStamina(float staminaAmount)
    {
        currentStamina -= staminaAmount;

        if (currentStamina <= 0)
        {
            // Player is out of stamina, handle exhaustion here
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy2 enemy = other.gameObject.GetComponent<Enemy2>();
            TakeDamage(enemy.GetDamage());
        }
    }
}