using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// ONLY FOR SAMPLE SCENE!
// EnemyIndicator Tag on each Indicator
public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyPrefabMelee;
    public GameObject enemyPrefabRanged;
    public GameObject enemyPrefabaggroDwarf;
    public GameObject freundlicherSmiley;
    public GameObject crab;

    GameObject[] allEnemies;
    GameObject[] allEnemyIndicators;


    public void Start()
    {
        allEnemyIndicators = GameObject.FindGameObjectsWithTag("EnemyIndicator");
        //Debug.Log(allEnemyIndicators.Length + " = length");

        SpawnEnemyType(enemyPrefabMelee, "Melee");

        SpawnEnemyType(enemyPrefabRanged, "Ranged");

        SpawnEnemyType(enemyPrefabaggroDwarf, "AggroDwarf");

        SpawnEnemyType(freundlicherSmiley, "FreundlicherSmiley");

        SpawnEnemyType(crab, "Crab");


        // assign groupNumber to Enemy                  // or use alert-range instead of groupNumber?
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // every Enemy using groupNumbers should have the tag "Enemy"
        foreach (GameObject allE in allEnemies)
        {
            foreach (GameObject ind in allEnemyIndicators)
            {
                if (allE.gameObject.transform.position == ind.gameObject.transform.position)
                {
                    allE.gameObject.GetComponent<EnemyStats>().groupNumber = ind.gameObject.GetComponent<EnemyGroup>().groupNumber;
                }
            }
        }

        GameObject.Find("Enemy Position Indicators").SetActive(false);
    }

    void SpawnEnemyType(GameObject prefab, string indicatorEnemyName)
    {
        GameObject[] allWithName = new GameObject[allEnemyIndicators.Length];
        int i = 0;
        foreach (GameObject allInd in allEnemyIndicators)
        {
            if (allInd.GetComponent<EnemyGroup>().enemyName == indicatorEnemyName)
            {
                allWithName[i] = allInd;
                i++;
            }
        }

        foreach (GameObject allName in allWithName)
        {
            if (allName != null)
            {
                Vector2 pos = allName.transform.position;
                PhotonNetwork.InstantiateRoomObject(prefab.name, pos, Quaternion.identity);
            }
        }
    }

    //public GameObject enemyPrefabMelee;
    //public GameObject enemyPrefabRanged;

    //Vector2 position1 = new Vector2(-30, 13);
    //Vector2 position2 = new Vector2(-28, 13);
    //Vector2 position3 = new Vector2(-26, 13);
    //Vector2 position4 = new Vector2(-24, 13);
    //Vector2 position5 = new Vector2(-22, 13);

    //public void Start()
    //{
    //    PhotonNetwork.InstantiateRoomObject(enemyPrefabMelee.name, position1, Quaternion.identity); // Gegner 1 Melee
    //    PhotonNetwork.InstantiateRoomObject(enemyPrefabMelee.name, position2, Quaternion.identity); // Gegner 2 Melee
    //    PhotonNetwork.InstantiateRoomObject(enemyPrefabMelee.name, position3, Quaternion.identity); // Gegner 3 Melee
    //    PhotonNetwork.InstantiateRoomObject(enemyPrefabRanged.name, position4, Quaternion.identity); // Gegner 4 Ranged
    //    PhotonNetwork.InstantiateRoomObject(enemyPrefabRanged.name, position5, Quaternion.identity); // Gegner 5 Ranged
    //}
}