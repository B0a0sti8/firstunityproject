using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyPrefabMelee;
    public GameObject enemyPrefabRanged;

    Vector2 position1 = new Vector2(-45, 7);
    Vector2 position2 = new Vector2(-45, 8);
    Vector2 position3 = new Vector2(-45, 9);
    Vector2 position4 = new Vector2(-45, 10);
    Vector2 position5 = new Vector2(-45, 11);

    public void Start()
    {
        PhotonNetwork.InstantiateRoomObject(enemyPrefabMelee.name, position1, Quaternion.identity); // Gegner 1 Melee
        PhotonNetwork.InstantiateRoomObject(enemyPrefabMelee.name, position2, Quaternion.identity); // Gegner 2 Melee
        PhotonNetwork.InstantiateRoomObject(enemyPrefabMelee.name, position3, Quaternion.identity); // Gegner 3 Melee
        PhotonNetwork.InstantiateRoomObject(enemyPrefabRanged.name, position4, Quaternion.identity); // Gegner 4 Ranged
        PhotonNetwork.InstantiateRoomObject(enemyPrefabRanged.name, position5, Quaternion.identity); // Gegner 5 Ranged
    }
}