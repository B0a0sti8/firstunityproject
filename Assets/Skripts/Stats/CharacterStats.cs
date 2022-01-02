using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterStats : MonoBehaviourPunCallbacks
{
    public PhotonView view;

    public Stat maxHealth;
    public float currentHealth;

    public Stat damage;
    public Stat armor;
    public Stat evade;

    public Stat movementSpeed;

    public bool isAlive;



    public virtual void TakeDamage(float damage, int missRandomRange)
    {
        //Debug.Log("damage: " + damage);
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, float.MaxValue); // if (damage < 0) { damage = 0 }

        if (missRandomRange <= Mathf.Clamp(evade.GetValue(), 0, 100))
        {
            Debug.Log("MISS:" + missRandomRange + " <= " + Mathf.Clamp(evade.GetValue(), 0, 100));
        }
        else
        {
            if (view.IsMine)
            {
                FindObjectOfType<AudioManager>().Play("Oof");
            }
            currentHealth -= damage;
            Debug.Log(gameObject.transform.position);
            DamagePopup.Create(gameObject.transform.position, (int)damage, false);
            if (currentHealth <= 0) // maybe put in update instead
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
