using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    private List<QuestScript> questScripts = new List<QuestScript>();

    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questParent;

    private Quest selected;

    [SerializeField] private TextMeshProUGUI questDescription;

    private static QuestLog instance;

    public static QuestLog MyInstance 
    {
        get 
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<QuestLog>();
            }
            return instance;
        } 
        
    }

    // Start is called before the first frame update
    void Start()
    {
        questDescription = transform.Find("Description").Find("TextArea").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AcceptQuest(Quest quest)
    {
        foreach (CollectObjective o in quest.MyCollectObjectives)
        {
            InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(o.UpdateItemCount);
        }

        GameObject go = Instantiate(questPrefab, questParent);

        QuestScript qs = go.GetComponent<QuestScript>();
        quest.MyQuestScript = qs;
        qs.MyQuest = quest;
        questScripts.Add(qs);

        go.GetComponent<TextMeshProUGUI>().text = quest.MyTitle;
    }

    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.DeSelect();
            }

            string objectives = string.Empty;
            selected = quest;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            string title = quest.MyTitle;
            questDescription.text = string.Format("<b>{0}</b>\n\n<size=14>{1}</size>\n\nObjectives\n<size=14>{2}</size>", title, quest.MyDescription, objectives);
        }
    }

    public void CheckCompletion()
    {
        foreach (QuestScript qs in questScripts)
        {
            qs.IsComplete();
        }
    }
}
