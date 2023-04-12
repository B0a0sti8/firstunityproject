using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnTrainingsDummies : NetworkBehaviour
{
    [SerializeField] GameObject trainingsDummy;
    [SerializeField] Vector3 spawnPos1;
    [SerializeField] Vector3 spawnPos2;
    [SerializeField] Vector3 spawnPos3;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        {
            GameObject go = Instantiate(trainingsDummy, spawnPos1, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();

            GameObject go1 = Instantiate(trainingsDummy, spawnPos2, Quaternion.identity);
            go1.GetComponent<NetworkObject>().Spawn();

            GameObject go2 = Instantiate(trainingsDummy, spawnPos3, Quaternion.identity);
            go2.GetComponent<NetworkObject>().Spawn();
        }
    }
}
