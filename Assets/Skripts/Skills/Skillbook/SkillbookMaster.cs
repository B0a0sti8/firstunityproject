using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private bool hasToArangeTextField;
    private int hasToArangeTextFieldCounter;



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
        hasToArangeTextFieldCounter = 0;
        hasToArangeTextField = true;

        //if (classNameMain.ToString() == "Dummy")
        //{ return; }
    }

    // Nachdem Die Klassenin einer Vertical LayoutGroup liegen, ändert sich ihre Position am Ende eines Frame-Updates. Um dann die Position der TextFelder mit den 
    // Klassennamen anzupassen, warten wir ein Update und machen es dann.
    private void Update()
    {
        if (!hasToArangeTextField) return;

        if (hasToArangeTextFieldCounter > 1) { hasToArangeTextField = false; return; }

        hasToArangeTextFieldCounter++;

        List<string> clNames = new List<string>();
        clNames.Add(classNameMain); clNames.Add(classNameLeft); clNames.Add(classNameRight);

        int classNameTextFieldNum = 0;
        foreach (string clName in clNames)
        {
            if (clName == "" || clName == "Dummy") continue;
            classNameTextFieldNum += 1;
            Transform myTextField = skillbook.transform.Find("MyClassNamesText").Find("ClassName" + classNameTextFieldNum.ToString());

            allClassesSkills = skillbook.transform.Find("Classes").Find(clName.ToString() + "Skills");
            //Debug.Log(myTextField);
            //Debug.Log(myTextField.GetComponent<TextMeshProUGUI>());
            //Debug.Log(allClassesSkills);
            if (!allClassesSkills.gameObject.activeSelf) { myTextField.GetComponent<TextMeshProUGUI>().text = ""; return; }

            myTextField.GetComponent<TextMeshProUGUI>().text = clName;
            myTextField.localPosition = allClassesSkills.localPosition - new Vector3(650f, 0f, 0f);
        }
    }
}
