using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public Stat health;
    public Stat damage;
    public Stat armor;
    public Stat evade;

    public float currentHealth;

    private void Spawn()
    {
        currentHealth = health.GetValue();
    }

    public virtual void TakeDamage(float damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, float.MaxValue);

        if (Random.Range(0, 100) <= Mathf.Clamp(evade.GetValue(), 0, 100))
        {
            return;
        }

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
