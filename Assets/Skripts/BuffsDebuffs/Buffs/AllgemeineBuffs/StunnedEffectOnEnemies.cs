using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class StunnedEffectOnEnemies : Buff
{
    public int insectCount;
    //[SerializeField] private GameObject summonerInsect;
    CharacterStats myCurrentEnemyStats;

    public override void StartBuffEffect(CharacterStats enemyStats)
    {
        buffName = "Summon Insects On Death";
        buffDescription = "Insects will spawn when this enemy dies";
        base.StartBuffEffect(enemyStats);
        isRemovable = false;
        myCurrentEnemyStats = enemyStats;

        if (enemyStats.gameObject.GetComponent<EnemyAI>() != null)
        {
            enemyStats.gameObject.GetComponent<EnemyAI>().listOfStunSources.Add(this);
            enemyStats.gameObject.GetComponent<EnemyAI>().SetState(EnemyAI.State.DoNothing);
        }
    }

    public override void EndBuffEffect(CharacterStats enemyStats)
    {
        base.EndBuffEffect(enemyStats);

        if (enemyStats.gameObject.GetComponent<EnemyAI>() != null)
        {
            enemyStats.gameObject.GetComponent<EnemyAI>().listOfStunSources.Remove(this);
            if (enemyStats.gameObject.GetComponent<EnemyAI>().listOfStunSources.Count == 0)
            {
                enemyStats.gameObject.GetComponent<EnemyAI>().SetState(EnemyAI.State.Idle);
            }
        }
    }

    public override Buff Clone()
    {
        StunnedEffectOnEnemies clone = (StunnedEffectOnEnemies)this.MemberwiseClone();
        return clone;
    }
}
