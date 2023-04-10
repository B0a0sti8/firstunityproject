using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnTrainingsDummies : MonoBehaviour
{
    [SerializeField] GameObject trainingsDummy; 
    private void Start()
    {
        {
            GameObject go = Instantiate(trainingsDummy, new Vector3(-9.85f, -0.56f, 50.0f), Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();

            GameObject go1 = Instantiate(trainingsDummy, new Vector3(-11.05f, 1.41f, 50.0f), Quaternion.identity);
            go1.GetComponent<NetworkObject>().Spawn();

            GameObject go2 = Instantiate(trainingsDummy, new Vector3(-9.61f, -3.82f, 50.0f), Quaternion.identity);
            go2.GetComponent<NetworkObject>().Spawn();
        }
    }
}
