using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public Stat health;
    public Stat damage;
    public Stat armor;
    public Stat evade;

    float currentHealth;

    private void Spawn()
    {
        currentHealth = health.GetValue();
    }

    public void TakeDamage(float damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, float.MaxValue);

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // To be overwritten in Child Class
    }

}
