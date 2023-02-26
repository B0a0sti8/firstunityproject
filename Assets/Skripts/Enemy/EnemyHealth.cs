using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public HealthBar healthBar; // drag in the healthbar (either HUD or following)
    public EnemyStats enemyStats;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = enemyStats.maxHealth.GetValue();
        currentHealth = enemyStats.currentHealth.Value;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetMaxHealth((int)maxHealth);
        healthBar.SetHealth((int)currentHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }



    /*

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
    private void OnTakeDamage(InputValue value)
    {
        TakeDamage(5);
        FindObjectOfType<AudioManager>().Play("Oof");
    }*/
}
