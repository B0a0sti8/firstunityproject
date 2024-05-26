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
        myTalentTree = PLAYER.transform.Find("Own Canvases").Find("CanvasTalentTree").GetComponent<TalentTree>();

        if (playerStats.mainClassName == "" || myTalentTree.subClassMain == "") { playerStats.mainClassName = "Dummy"; myTalentTree.subClassMain = "Dummy"; }
        if (playerStats.leftSubClassName == "" || myTalentTree.subClassLeft == "") { playerStats.leftSubClassName = "Dummy"; myTalentTree.subClassLeft = "Dummy"; }
        if (playerStats.rightSubClassName == "" || myTalentTree.subClassRight == "") { playerStats.rightSubClassName = "Dummy"; myTalentTree.subClassRight = "Dummy"; }

        ResetClasses();

        if (classes.Find(playerStats.mainClassName) != null) classes.Find(playerStats.mainClassName).gameObject.SetActive(true);

        if (classes.Find(playerStats.rightSubClassName) != null) classes.Find(playerStats.rightSubClassName).gameObject.SetActive(true);

        if (classes.Find(playerStats.leftSubClassName) != null) classes.Find(playerStats.leftSubClassName).gameObject.SetActive(true);

        mySkillBook.UpdateCurrentSkills();
        playerStats.HandleResetMinionCount();
    }

    public void ChangeAndSetClass(string whichOne, string newClass)
    {
        bool isTalentTreeActive = myTalentTree.gameObject.activeSelf; // Beim Spieler laden wird der Talenttree geupdated ohne offen zu sein. Der Trick um fehler zu vermeiden ist, ihn innerhalb eines Frames zu öffnen, updaten und wieder zu schließen.
        Debug.Log("Changing Class.  " + whichOne + "  " + newClass);
        ResetClasses();

        switch (whichOne)
        {
            case "main":
                playerStats.mainClassName = newClass;
                myTalentTree.subClassMain = newClass;
                break;

            case "right":
                playerStats.rightSubClassName = newClass;
                myTalentTree.subClassRight = newClass;
                break;

            case "left":
                playerStats.leftSubClassName = newClass;
                myTalentTree.subClassLeft = newClass;
                break;
        }

        //myTalentTree.gameObject.SetActive(true);
        //myTalentTree.ResetSkillTree();
        //myTalentTree.ResetTalents();
        //myTalentTree.HasToCheckAfterReset();
        //myTalentTree.gameObject.SetActive(isTalentTreeActive);

        if (classes.Find(playerStats.mainClassName) != null) classes.Find(playerStats.mainClassName).gameObject.SetActive(true);
        if (classes.Find(playerStats.rightSubClassName) != null) classes.Find(playerStats.rightSubClassName).gameObject.SetActive(true);
        if (classes.Find(playerStats.leftSubClassName) != null) classes.Find(playerStats.leftSubClassName).gameObject.SetActive(true);
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