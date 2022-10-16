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
    public string className;


    void Start()
    {
        className = transform.parent.parent.GetComponent<PlayerStats>().className;
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
        className = transform.parent.parent.GetComponent<PlayerStats>().className;
        Debug.Log("1");
        if (className.ToString() == "")
        { return; }
        Debug.Log("1");

        allClassesSkills = skillbook.transform.Find("Classes").Find(className.ToString() + "Skills");
        currentSkills.Clear();
        actionSkillSlots.Clear();
        currentActiveSkillNames.Clear();
        for (int i = 0; i < actionSkill.childCount; i++)
        {
            actionSkillSlots.Add(actionSkill.GetChild(i).gameObject);
        }

        for (int i = 0; i < allClassesSkills.transform.childCount; i++)
        {
            if (allClassesSkills.GetChild(i).gameObject.GetComponent<Button>().enabled == true)
            {
                currentSkills.Add(allClassesSkills.GetChild(i).gameObject);
                allClassesSkills.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
            else
            {
                allClassesSkills.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
            }
        }

        foreach (GameObject currentSkill in currentSkills)
        {
            currentActiveSkillNames.Add(currentSkill.name);
        }

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
    }
}
