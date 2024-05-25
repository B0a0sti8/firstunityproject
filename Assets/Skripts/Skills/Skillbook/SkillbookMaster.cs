using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillbookMaster : MonoBehaviour
{
    public GameObject skillbook;
    public Transform allClassesSkills;
    public List<GameObject> currentSkills;
    public Transform actionSkill;
    public List<GameObject> actionSkillSlots;
    public string skillName;
    public List<string> currentActiveSkillNames;
    private string classNameMain;
    private string classNameLeft;
    private string classNameRight;



    void Start()
    {
        classNameMain = transform.parent.parent.GetComponent<PlayerStats>().mainClassName;
        skillbook = transform.Find("Skillbook").gameObject;
        actionSkill = transform.parent.Find("Canvas Action Skills").Find("SkillSlots");
        UpdateCurrentSkills();
        skillbook.SetActive(false);
    }

    public void OpenSkillbook()
    {
        if (skillbook.activeInHierarchy)
        {
            skillbook.SetActive(false);
        }
        else
        {
            skillbook.SetActive(true);
            UpdateCurrentSkills();
        }
    }

    public void UpdateCurrentSkills()
    {
        classNameMain = transform.parent.parent.GetComponent<PlayerStats>().mainClassName;
        classNameLeft = transform.parent.parent.GetComponent<PlayerStats>().leftSubClassName;
        classNameRight = transform.parent.parent.GetComponent<PlayerStats>().rightSubClassName;

        // Setzt erstmal alle Klassen auf aus und reseted alle Skillslots.
        for (int i = 0; i < skillbook.transform.Find("Classes").childCount; i++) { skillbook.transform.Find("Classes").GetChild(i).gameObject.SetActive(false); } 
        currentSkills.Clear();
        actionSkillSlots.Clear();
        currentActiveSkillNames.Clear();

        // Holt sich alles AktionSkillSlots in die Liste actionSkillSlots.
        for (int i = 0; i < actionSkill.childCount; i++) { actionSkillSlots.Add(actionSkill.GetChild(i).gameObject); }

        List<string> clNames = new List<string>();
        clNames.Add(classNameMain); clNames.Add(classNameLeft); clNames.Add(classNameRight);

        foreach (string clName in clNames)
        {
            Debug.Log(clName);
            if (clName == "" || clName == "Dummy") continue;

            allClassesSkills = skillbook.transform.Find("Classes").Find(clName.ToString() + "Skills");

            for (int i = 0; i < allClassesSkills.transform.childCount; i++)
            {
                if (allClassesSkills.GetChild(i).gameObject.GetComponent<Button>().enabled == true)
                {
                    currentSkills.Add(allClassesSkills.GetChild(i).gameObject);
                    allClassesSkills.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
                else allClassesSkills.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
            }
            allClassesSkills.gameObject.SetActive(true);
            Debug.Log(currentSkills.Count);
        }


        foreach (GameObject currentSkill in currentSkills) { currentActiveSkillNames.Add(currentSkill.name); }

        // Übergibt die Änderungen an alle ActionSkillSlots.
        foreach (GameObject actionSkillSlot in actionSkillSlots)
        {
            skillName = actionSkillSlot.GetComponent<ActionButton>().skillName;
            ColorBlock cb = actionSkillSlot.GetComponent<Button>().colors;
            if (currentActiveSkillNames.Contains(skillName))
            {
                actionSkillSlot.GetComponent<Button>().enabled = true;
                actionSkillSlot.GetComponent<ActionButton>().skillAvailable = true;
                cb.normalColor = new Color32(255, 255, 255, 255);
                actionSkillSlot.GetComponent<Button>().colors = cb;
            }
            else
            {
                actionSkillSlot.GetComponent<Button>().enabled = false;
                actionSkillSlot.GetComponent<ActionButton>().skillAvailable = false;
                cb.normalColor = new Color32(120, 120, 120, 255);
                actionSkillSlot.GetComponent<Button>().colors = cb;
            }
        }


        //if (classNameMain.ToString() == "Dummy")
        //{ return; }
    }
}
