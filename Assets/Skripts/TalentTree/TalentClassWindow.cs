using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentClassWindow : MonoBehaviour
{
    public string subClassPosition;
    TalentTree myTalentTree;
    ClassAssignment myClassManager;
    SkillbookMaster mySkillBook;

    void Start()
    {
        myTalentTree = transform.parent.GetComponent<TalentTree>();
        myClassManager = transform.parent.parent.parent.Find("CanvasClassChoice").GetComponent<ClassAssignment>();
        mySkillBook = transform.parent.parent.parent.Find("Canvas Skillbook").GetComponent<SkillbookMaster>();
    }

    public void ChangeClassToButtonString(string newClassName)
    {
        if (subClassPosition == "Main")
        { 
            myTalentTree.subClassMain = newClassName;
            Debug.Log(newClassName);
            myClassManager.ChangeAndSetClass("main", newClassName);
        }

        if (subClassPosition == "Left")
        { 
            myTalentTree.subClassLeft = newClassName;
            myClassManager.ChangeAndSetClass("left", newClassName);
        }

        if (subClassPosition == "Right")
        { 
            myTalentTree.subClassRight = newClassName;
            myClassManager.ChangeAndSetClass("right", newClassName);
        }

        mySkillBook.UpdateCurrentSkills();

        CloseWindow();

        myTalentTree.ResetSkillTree();
        myTalentTree.ResetTalents();
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}