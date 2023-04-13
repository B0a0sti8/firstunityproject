using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    float timer = 0f;
    public float baseAttackDamage = 0;

    public void StartEnemyAtk(GameObject target)
    {
        if (gameObject.GetComponent<EnemyStats>().isAlive.Value && target.gameObject.GetComponent<CharacterStats>().isAlive.Value)
        {
            if (timer <= 0)
            { // inSight Check???
                timer = 1 / gameObject.GetComponent<EnemyStats>().actionSpeed.GetValue();
                EnemyAtkEffect(target);
            }
        }
    }

    public virtual void EnemyAtkEffect(GameObject target)
    {
    }


    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer < 0)
        {
            timer = 0;
        }
    }
}
