using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAssignment : MonoBehaviour
{
    GameObject PLAYER;
    //GameObject blueMage;
    //GameObject warrior;
    //GameObject hunter;
    //GameObject priest;
    //GameObject mage;
    //GameObject paladin;
    PlayerStats playerStats;
    TalentTree myTalentTree;
    Transform classes;
    SkillbookMaster mySkillBook;
    int classCount;


    public void Start()
    {
        PLAYER = transform.parent.parent.gameObject;
        playerStats = PLAYER.GetComponent<PlayerStats>();
        classes = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes");
        mySkillBook = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>();
        myTalentTree = PLAYER.transform.Find("Own Canvases").Find("CanvasTalentTree").Find("TalentTreeWindow").GetComponent<TalentTree>();

        playerStats.mainClassName = "Warrior";
        playerStats.leftSubClassName = "Summoner";
        playerStats.rightSubClassName = "Dummy";

        myTalentTree.subClassMain = "Warrior";
        myTalentTree.subClassLeft = "Summoner";
        myTalentTree.subClassRight = "Dummy";

        ResetClasses();

        if (classes.Find(playerStats.mainClassName) != null)
        {
            classes.Find(playerStats.mainClassName).gameObject.SetActive(true);
        }

        if (classes.Find(playerStats.rightSubClassName) != null)
        {
            classes.Find(playerStats.rightSubClassName).gameObject.SetActive(true);
        }

        if (classes.Find(playerStats.leftSubClassName) != null)
        {
            classes.Find(playerStats.leftSubClassName).gameObject.SetActive(true);
        }

        mySkillBook.UpdateCurrentSkills();
        playerStats.HandleResetMinionCount();
    }

    public void ChangeAndSetClass(string whichOne, string newClass)
    {
        ResetClasses();

        switch (whichOne)
        {
            case "main":
                playerStats.mainClassName = newClass;
                    break;

            case "right":
                playerStats.rightSubClassName = newClass;
                break;

            case "left":
                playerStats.leftSubClassName = newClass;
                break;
        }

        if (classes.Find(playerStats.mainClassName) != null)
        {
            classes.Find(playerStats.mainClassName).gameObject.SetActive(true);
        }

        if (classes.Find(playerStats.rightSubClassName) != null)
        {
            classes.Find(playerStats.rightSubClassName).gameObject.SetActive(true);
        }

        if (classes.Find(playerStats.leftSubClassName) != null)
        {
            classes.Find(playerStats.leftSubClassName).gameObject.SetActive(true);
        }
    }

    public void ResetClasses()
    {
        transform.parent.Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
        classCount = classes.childCount;
        for (int i = 0; i < classCount; i++)
        {
            classes.GetChild(i).gameObject.SetActive(false);
        }
    }
}