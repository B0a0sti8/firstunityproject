using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float X;
    public float Y;

    public void Start()
    {
        Vector2 Position = new Vector2(X, Y);
        PhotonNetwork.InstantiateRoomObject(enemyPrefab.name, Position, Quaternion.identity);
    }
}