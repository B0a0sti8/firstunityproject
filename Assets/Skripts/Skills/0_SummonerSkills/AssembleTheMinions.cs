using Unity.Netcode;
using UnityEngine;

public class AssembleTheMinions : SkillPrefab
{
    [SerializeField] private GameObject imp;
    [SerializeField] private GameObject insect;
    [SerializeField] private GameObject spiritWolf;
    SummonerClass mySummonerClass;

    private float minionBaseDamage;

    int minionCount;
    float minionLifeTimeBase;

    public override void Start()
    {
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
        animationTime = 5f;

        base.Start();
        myClass = "Summoner";
        hasGlobalCooldown = true;
        needsTargetEnemy = true;
        skillRange = 20;
        ownCooldownTimeBase = 120f;
        castTimeOriginal = 2f;
        isSkillChanneling = false;

        minionBaseDamage = 10f;

        minionLifeTimeBase = 10;
        myAreaType = AreaType.SingleTarget;
    }

    public override void StartCasting()
    {
        PlaySkillAnimation("Summoner", "Summoner_summon2");
        base.StartCasting();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        float minionLifeTime = (minionLifeTimeBase + mySummonerClass.increasedMinionDuration) * playerStats.skillDurInc.GetValue();
        float minionDamage = minionBaseDamage * (1 + mySummonerClass.increasedMinionDamage);
        NetworkObjectReference playerReference = (NetworkObjectReference)PLAYER;
        NetworkObjectReference enemyReference = (NetworkObjectReference)currentTargets[0];

        minionCount = mySummonerClass.assembleTheMinionsCount;
        for (int i = 0; i < minionCount * 3; i++)
        {
            if ((i+3)%3 == 0) SpawnMinionsServerRpc(enemyReference, playerReference, minionDamage * 1.2f, minionLifeTime, i); // imp
            if((i + 3) % 3 == 1) SpawnMinionsServerRpc(enemyReference, playerReference, minionDamage * 1.0f, minionLifeTime, i); // insect
            if ((i + 3) % 3 == 2) SpawnMinionsServerRpc(enemyReference, playerReference, minionDamage * 1.5f, minionLifeTime, i); // SpiritWolf
            mySummonerClass.SummonerClass_OnMinionSummoned();
        }
    }

    [ServerRpc]
    private void SpawnMinionsServerRpc(NetworkObjectReference targetEnemy, NetworkObjectReference summoningPlayer, float minionDamage, float minionDuration, int num)
    {
        // Holt sich den Summoning Player aus der Network-Referenz
        //Debug.Log("Summon Imp Server RPC!");
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        targetEnemy.TryGet(out NetworkObject targE);
        GameObject targEn = targE.gameObject;

        // Erzeugt zufällige Koordinaten
        float x = Random.Range(2f, 5f);
        float y = Random.Range(2f, 5f);
        float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

        if (sumPla != null)
        {
            // Baut aus den zufälligen Koordinaten den Spawnpunkt des Imps. Setzt das Herrchen fest und setzt den Imp in den Kampf. Spawnt den Imp Serverseitig
            Vector2 posi = (Vector2)targEn.transform.position + new Vector2(x * signx, y * signy);

            GameObject myMinionObject = new GameObject();

            if ((num + 3) % 3 == 0) myMinionObject = Instantiate(imp, posi, Quaternion.identity); // imp
            if ((num + 3) % 3 == 1) myMinionObject = Instantiate(insect, posi, Quaternion.identity); // insect
            if ((num + 3) % 3 == 2) myMinionObject = Instantiate(spiritWolf, posi, Quaternion.identity); // SpiritWolf

            myMinionObject.GetComponent<NetworkObject>().Spawn();
            myMinionObject.GetComponent<MinionPetAI>().isInFight = true;
            myMinionObject.GetComponent<HasLifetime>().maxLifetime = minionDuration;
            //Debug.Log("MyImpLifetime = " + impDuration);
            myMinionObject.GetComponent<MeleeEnemyAttackTest>().baseAttackDamage = minionDamage;

            myMinionObject.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
            sumPla.GetComponent<PlayerStats>().myMinions.Add(myMinionObject);
            //Debug.Log("Added Minion as Server" + sumPla.GetComponent<PlayerStats>().myMinions.Count);

            NetworkObjectReference minionRef = (NetworkObjectReference)myMinionObject;

            // Die ClientRpc sagt allen Clients, wer das Herrchen ist und dass das Herrchen ein Pet hat.
            SummonMinionsClientRpc(summoningPlayer, minionRef);
        }
    }

    [ClientRpc]
    private void SummonMinionsClientRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference minionRef)
    {
        // Die ClientRpc sagt allen Clients, wer das Herrchen ist und dass das Herrchen ein Pet hat.
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        minionRef.TryGet(out NetworkObject minio);
        GameObject miniono = minio.gameObject;

        miniono.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
        miniono.GetComponent<MinionPetAI>().isInFight = true;
    }
}
