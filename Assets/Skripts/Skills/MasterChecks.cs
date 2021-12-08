using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// AnimTime = GCEarlyTime = OwnCooldownEarlyTime
// GCTime >= GCEarlyTime
// ownCooldownTime >= ownCooldownEarlyTime

public class MasterChecks : MonoBehaviour
{
    public bool masterAnimationActive = false;
    public float masterAnimTime = 0.5f;
    public float masterAnimTimeLeft;

    public bool masterGCActive = false;
    public float masterGCTime = 1.5f;
    public float masterGCTimeLeft;
    public float masterGCEarlyTime = 0.5f;

    public float masterOwnCooldownEarlyTime = 0.5f;

    public bool masterIsSkillInQueue = false;

    GameObject[] globalCooldownSkills;
    //GameObject[] textGameObjects;

    void Awake()
    {
        globalCooldownSkills = GameObject.FindGameObjectsWithTag("GlobalCooldownSkill");
        //textGameObjects = GameObject.FindGameObjectsWithTag("WeaponSkillCDText");
    }

    void Update()
    {
        if (masterAnimTimeLeft > 0)
        {
            masterAnimTimeLeft -= Time.deltaTime;
        }
        else // time <= 0
        {
            if (masterAnimationActive)
            {
                masterAnimationActive = false;
                masterAnimTimeLeft = 0f;
            }
        }

        if (masterGCTimeLeft > 0)
        {
            masterGCTimeLeft -= Time.deltaTime;
        }
        else // time <= 0
        {
            if (masterGCActive)
            {
                masterGCActive = false;
                foreach (GameObject gObj in globalCooldownSkills) //color all GCSkills normal
                {
                    gObj.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
                masterGCTimeLeft = 0f;
            }
        }
    }
}
