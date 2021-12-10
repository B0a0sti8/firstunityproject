using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    //public int maxHealth = 100;
    //public int currentHealth;

    //public int maxMana = 1000;
    //public int currentMana;

    //public HealthBar healthBar; // drag in the healthbar (either HUD or following)
    //public ManaBar manaBar; // drag in the healthbar (either HUD or following)

    //float tickEveryXSeconds = 1f;
    //float tickEveryXSecondsTimer = 0f;


    //void Start()
    //{
    //    currentHealth = maxHealth;
    //    currentMana = maxMana;
    //}

    private void Update()
    {
        // updates Bars on Canvas
        //healthBar.SetMaxHealth(maxHealth);
        //manaBar.SetMaxMana(maxMana);
        //healthBar.SetHealth(currentHealth);
        //manaBar.SetMana(currentMana);

        //if (currentHealth > maxHealth)
        //{
        //    currentHealth = maxHealth;
        //}
        //if (currentMana > maxMana)
        //{
        //    currentMana = maxMana;
        //}

        //if (currentHealth < 0)
        //{
        //    currentHealth = 0;
        //}
        //if (currentMana < 0)
        //{
        //    currentMana = 0;
        //}

        //tickEveryXSecondsTimer += Time.deltaTime;
        //if (tickEveryXSecondsTimer >= tickEveryXSeconds)
        //{
        //    tickEveryXSecondsTimer = 0f;
        //    currentMana += 25;
        //}
    }

    //private void OnTakeDamage() // when pressing space
    //{
    //    gameObject.GetComponent<PlayerStats>().TakeDamage(5);
    //    //TakeDamage(5);
    //}

    //public void TakeDamage(int damage)
    //{
    //    currentHealth -= damage;

    //    // healthBar.SetHealth(currentHealth);
    //}







    /*
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
    */
}
