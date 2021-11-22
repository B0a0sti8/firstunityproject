using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar; // drag in the healthbar (either HUD or following)

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    private void OnTakeDamage(InputValue value)
    {
        TakeDamage(5);
        FindObjectOfType<AudioManager>().Play("Oof");
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // healthBar.SetHealth(currentHealth);
    }
}
