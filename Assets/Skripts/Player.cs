using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar; // drag in the healthbar (either HUD or following)

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        maxHealth = data.maxHealth;
        currentHealth = data.currentHealth;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
    }

    void Start()
    {
        currentHealth = maxHealth;
        // healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
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
