using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    public float currentHealth;

    public Stat damage;
    public Stat armor;
    public Stat evade;

    public bool isAlive;


    public virtual void TakeDamage(float damage)
    {
        //Debug.Log("damage: " + damage);
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, float.MaxValue);
        //Debug.Log(evade.GetValue());
        if (Random.Range(0, 100) <= Mathf.Clamp(evade.GetValue(), 0, 100))
        {
            Debug.Log("MISS");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Oof");
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        FindObjectOfType<AudioManager>().Play("OoOof");
        //Debug.Log("He dead");
        isAlive = false;
        // To be overwritten in Child Class
    }
}
