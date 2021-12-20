using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public static SpawnPlayers instance;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public bool isSpawned = false;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; // make sure no more code is called, before destroying the object
        }
    }

    public void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        if (!isSpawned)
         {
            PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
            isSpawned = true;
        }
    }
}