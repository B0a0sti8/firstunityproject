using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesDungeon1 : MonoBehaviour
{
    public GameObject enemyPrefabMelee;
    public GameObject enemyPrefabRanged;
    public GameObject enemyPrefabBoss;

    GameObject[] allEnemyIndicators;
    GameObject[] allEnemies;


    public void Start()
    {
        allEnemyIndicators = GameObject.FindGameObjectsWithTag("EnemyIndicator");
        //Debug.Log(allEnemyIndicators.Length + " = length");

        SpawnEnemyType(enemyPrefabMelee, "Melee");

        SpawnEnemyType(enemyPrefabRanged, "Bowman");

        SpawnEnemyType(enemyPrefabBoss, "BigBoii");

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
                ///PhotonNetwork.InstantiateRoomObject(prefab.name, pos, Quaternion.identity);
            }
        }
    }
}
