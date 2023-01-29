using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    float timer = 0f;

    public void StartEnemyAtk(GameObject target)
    {
        if (gameObject.GetComponent<EnemyStats>().isAlive && target.gameObject.GetComponent<PlayerStats>().isAlive)
        {
            if (timer <= 0)
            { // inSight Check???
                if (Vector2.Distance(gameObject.transform.position, target.transform.position) <= gameObject.GetComponent<EnemyAI>().attackRange)
                {
                    timer = gameObject.GetComponent<EnemyStats>().baseAttackSpeed;
                    EnemyAtkEffect(target);
                }
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
