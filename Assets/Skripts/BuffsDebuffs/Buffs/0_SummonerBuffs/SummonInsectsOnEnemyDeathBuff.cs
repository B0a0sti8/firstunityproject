using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonInsectsOnEnemyDeathBuff : Buff
{
    public int insectCount;
    //[SerializeField] private GameObject summonerInsect;
    CharacterStats myCurrentEnemyStats;

    public override void StartBuffEffect(CharacterStats enemyStats)
    {
        buffName = "Summon Insects On Death";
        buffDescription = "Insects will spawn when this enemy dies";
        base.StartBuffEffect(enemyStats);
        //playerStats.actionSpeed.AddModifierAdd(value);
        isRemovable = false;
        myCurrentEnemyStats = enemyStats;

        enemyStats.onDeath += SpawnInsects;
    }

    public override void EndBuffEffect(CharacterStats enemyStats)
    {
        base.EndBuffEffect(enemyStats);
        enemyStats.actionSpeed.RemoveModifierAdd(value);
        enemyStats.onDeath -= SpawnInsects;
    }

    public override Buff Clone()
    {
        SummonInsectsOnEnemyDeathBuff clone = (SummonInsectsOnEnemyDeathBuff)this.MemberwiseClone();
        return clone;
    }

    void SpawnInsects()
    {
        // TickTime und TickValue werden hier als Anzahl der Insekten und als InsektenSchaden verwendet.

        Debug.Log("Spawning Insects.");
        Debug.Log(tickTime);
        Debug.Log((int)tickTime);
        buffSource.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonInsectsOnEnemyDeath>().SpawnInsectsServerRpc(buffSource.gameObject.GetComponent<NetworkObject>(), myCurrentEnemyStats.gameObject.GetComponent<NetworkObject>(), (int)additionalValue2, additionalValue1, additionalValue3);
    }
}
