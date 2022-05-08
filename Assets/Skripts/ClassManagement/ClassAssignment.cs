using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAssignment : MonoBehaviour
{
    GameObject PLAYER;
    GameObject blueMage;
    GameObject warrior;
    GameObject hunter;
    GameObject priest;
    GameObject mage;
    GameObject paladin;
    PlayerStats playerStats;
    Transform classes;
    int classCount;

    public void Awake()
    {
        PLAYER = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        playerStats = PLAYER.GetComponent<PlayerStats>();
        classes = PLAYER.transform.Find("Own Canvases").transform.Find("Canvas Skillbook").transform.Find("Skillbook").transform.Find("Classes").transform;
        blueMage = classes.Find("BlueMageSkills").gameObject;
        warrior = classes.Find("WarriorSkills").gameObject;
        hunter = classes.Find("HunterSkills").gameObject;
        priest = classes.Find("PriestSkills").gameObject;
        mage = classes.Find("MageSkills").gameObject;
        paladin = classes.Find("PaladinSkills").gameObject;
        DeactivateClass();
    }

    public void ChangeClassBlueMage()
    { 
        playerStats.className = "BlueMage";
        DeactivateClass();
        blueMage.SetActive(true);
    }

    public void ChangeClassWarrior()
    {
        playerStats.className = "Warrior";
        DeactivateClass();
        warrior.SetActive(true);
    }

    public void ChangeClassHunter()
    {
        playerStats.className = "Hunter";
        DeactivateClass();
        hunter.SetActive(true);
    }

    public void ChangeClassPriest()
    {
        playerStats.className = "Priest";
        DeactivateClass();
        priest.SetActive(true);
    }

    public void ChangeClassMage()
    {
        playerStats.className = "Mage";
        DeactivateClass();
        mage.SetActive(true);
    }

    public void ChangeClassPaladin()
    {
        playerStats.className = "Paladin";
        DeactivateClass();
        paladin.SetActive(true);
    }


    public void DeactivateClass()
    {
        classCount = classes.childCount;
        for (int i = 0; i < classCount; i++)
        {
            classes.GetChild(i).gameObject.SetActive(false);
        }
    }
}