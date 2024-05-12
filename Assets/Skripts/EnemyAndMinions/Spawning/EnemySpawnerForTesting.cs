using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemySpawnerForTesting : NetworkBehaviour
{
    float elapsed;
    float spawnTime;
    [SerializeField] GameObject meleeEnemy;
    List<GameObject> myEnemies;
    public bool spawnInf;

    private void Start()
    {
        spawnTime = 2f;
        myEnemies = new List<GameObject>();
        spawnInf = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;

        if (elapsed < spawnTime) elapsed += Time.deltaTime;
        else
        {
            elapsed = 0;
            SpawnMeleeEnemyTest();
        }
    }

    void SpawnMeleeEnemyTest()
    {
        if (spawnInf || myEnemies.Count < 4)
        {
            GameObject myMeleeEnemy = Instantiate(meleeEnemy, transform.position, Quaternion.identity);
            myMeleeEnemy.GetComponent<NetworkObject>().Spawn();
            myEnemies.Add(myMeleeEnemy);
        }
    }
}