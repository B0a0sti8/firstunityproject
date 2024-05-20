using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonAstralSnake : SkillPrefab
{
    public float damage = 400f;
    [SerializeField] GameObject mySnake;
    int snakeBounceCount;
    SummonerClass mySummonerClass;
    float bounceRange;
    List<GameObject> potSnaketargets;

    //[SerializeField] Sprite astralSnakeDoTBuffSprite;

    //SummonAstralSnakeDoT snakeDotBuff = new SummonAstralSnakeDoT();
    float snakeDoTDuration;
    float snakeDotTickTime;
    float snakeDotDamage;

    //SummonAstralSnakeDebuff snakeDebuff = new SummonAstralSnakeDebuff();
    float snakeDebuffDuration;
    float snakeDebuffValue;

    public override void Start()
    {
        snakeBounceCount = 5;
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
        castTimeOriginal = 1.5f;
        ownCooldownTimeBase = 30f;
        needsTargetEnemy = true;
        skillRange = 20;
        bounceRange = 15f;
        hasGlobalCooldown = true;

        potSnaketargets = new List<GameObject>();

        // Für den eventuellen Dot der Schlange
        snakeDoTDuration = 9f * playerStats.skillDurInc.GetValue();
        snakeDotTickTime = 3f;// * playerStats.tickRateMod.GetValue();
        snakeDotDamage = 100f;

        // Für den eventuellen Debuff der Schlange
        snakeDebuffDuration = 9f * playerStats.skillDurInc.GetValue();
        snakeDebuffValue = -0.05f * playerStats.debuffInc.GetValue();

        base.Start();
    }

    public override void Update()
    {
        tooltipSkillDescription = "Throws an astral snake that bounces between enemies, dealing damage and increasing damage taken. ";
        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        //Debug.Log("Firing DarkBolt");
        float tOA = 0.2f;

        GameObject myTarget = currentTargets[0];
        GameObject mySource = PLAYER;

        StartCoroutine(SnakeBouncer(myTarget, mySource, tOA, snakeBounceCount + mySummonerClass.astralSnakeAdditionalBounces -1));
        //Debug.Log("Summoning snake. Remaining bounces: " + (snakeBounceCount + mySummonerClass.astralSnakeAdditionalBounces) + "1");
    }

    [ServerRpc]
    private void SpawnAstralSnakeServerRpc(NetworkObjectReference myPlayerRef, float timeOfArrival, NetworkObjectReference targetRef)
    {
        myPlayerRef.TryGet(out NetworkObject myPlayer);
        targetRef.TryGet(out NetworkObject myTarget);

        GameObject project = Instantiate(mySnake, myPlayer.transform.position, Quaternion.identity);
        //project.GetComponent<NetworkObject>().Spawn();
        project.GetComponent<ProjectileFlyToTarget>().target = myTarget.transform;
        project.GetComponent<ProjectileFlyToTarget>().timeToArrive = timeOfArrival;

        SpawnAstralSnakeClientRpc(myPlayerRef, timeOfArrival, targetRef);
    }

    [ClientRpc]
    private void SpawnAstralSnakeClientRpc(NetworkObjectReference myPlayerRef, float timeOfArrival, NetworkObjectReference targetRef)
    {
        myPlayerRef.TryGet(out NetworkObject myPlayer);
        targetRef.TryGet(out NetworkObject myTarget);

        GameObject project = Instantiate(mySnake, myPlayer.transform.position, Quaternion.identity);
        project.GetComponent<ProjectileFlyToTarget>().target = myTarget.transform;
        project.GetComponent<ProjectileFlyToTarget>().timeToArrive = timeOfArrival;
    }

    IEnumerator DelayedDamage(GameObject target, float tOA)
    {
        yield return new WaitForSeconds(tOA);
        DealDamage(target, damage);

        // Wenn geskillt fügt die Schlange einen Dot zu.
        if (mySummonerClass.astralSnakeHasDoT)
        {
            target.GetComponent<BuffManagerNPC>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "SummonAstralSnakeDoT", false);
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(target.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "SummonAstralSnakeDoT", "SummonAstralSnakeDoT", true, snakeDoTDuration, snakeDotTickTime, snakeDotDamage * mySummonerClass.astralSnakeDotMod);
        }

        // Wenn geskillt fügt die Schlange einen Debuff zu.
        if (mySummonerClass.astralSnakeHasDebuff)
        {
            target.GetComponent<BuffManagerNPC>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "SummonAstralSnakeDebuff", false);
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(target.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "SummonAstralSnakeDebuff", "SummonAstralSnakeDebuff", false, snakeDebuffDuration, 0, snakeDebuffValue);
        }
    }

    IEnumerator SnakeBouncer(GameObject myTarget, GameObject mySource, float tOA, int remainingBounces)
    {
        //Debug.Log("Summoning snake. Remaining bounces: " + remainingBounces); 
        //Lässt eine Schlange zum Gegner fliegen und dealt nach Wartezeit damage.
        SpawnAstralSnakeServerRpc(mySource.GetComponent<NetworkObject>(), tOA, myTarget.GetComponent<NetworkObject>());
        StartCoroutine(DelayedDamage(myTarget, tOA));
        yield return new WaitForSeconds(tOA); // Wartet bis schlange angekommen ist.

        //Sucht alle Gegner in der Nähe des ursprünglichen Ziels.
        potSnaketargets.Clear();
        Collider2D[] hit = Physics2D.OverlapCircleAll(myTarget.transform.position, bounceRange, (1 << LayerMask.NameToLayer("Enemy")));
        foreach (Collider2D coll in hit)
        {
            potSnaketargets.Add(coll.gameObject);
        }

        // Wählt ein zufälliges Target für den nächsten Sprung. Und beginnt von vorne, solange es remainingBounces gibt.
        int randomInt = Random.Range(0, potSnaketargets.Count-1);
        GameObject myNewTarget = potSnaketargets[randomInt];
        GameObject myNewSource = myTarget;

        if (remainingBounces >0)
        {
            StartCoroutine(SnakeBouncer(myNewTarget, myNewSource, tOA, remainingBounces - 1));
        }
    }
}
