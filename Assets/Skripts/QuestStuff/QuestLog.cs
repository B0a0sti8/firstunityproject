using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class QuestLog : MonoBehaviour
{
    private List<QuestScript> questScripts = new List<QuestScript>();
    private List<Quest> quests = new List<Quest>();
    private StuffManagerScript stuffManager;

    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questParent;

    private Quest selected;

    [SerializeField] private TextMeshProUGUI questDescription;

    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Text QuestCountTxt;

    [SerializeField] private int maxCount;
    private int currentCount;

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

    void Start()
    {
        questDescription = transform.Find("Description").Find("TextArea").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        stuffManager = transform.parent.parent.parent.gameObject.GetComponent<StuffManagerScript>();
        QuestCountTxt.text = currentCount + "/" + maxCount;
    }

    public void AcceptQuest(Quest quest)
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            QuestCountTxt.text = currentCount + "/" + maxCount;
            foreach (CollectObjective o in quest.MyCollectObjectives)
            {
                InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(o.UpdateItemCount);
                o.UpdateItemCount();

            }

            foreach (KillObjective o in quest.MyKillObjectives)
            {
                stuffManager.killConfirmedEvent += new KillConfirmed(o.UpdateKillCount);

            }

            quests.Add(quest);

            GameObject go = Instantiate(questPrefab, questParent);

            QuestScript qs = go.GetComponent<QuestScript>();
            quest.MyQuestScript = qs;
            qs.MyQuest = quest;
            questScripts.Add(qs);

            go.GetComponent<TextMeshProUGUI>().text = quest.MyTitle;

            CheckCompletion();
        }
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

            foreach (Objective obj in quest.MyKillObjectives)
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
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
            qs.IsComplete();
        }
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha == 1)
        {
            Close();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void AbandonQuest()
    {
        foreach (CollectObjective o in selected.MyCollectObjectives)
        {
            InventoryScript.MyInstance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
        }

        foreach (KillObjective o in selected.MyKillObjectives)
        {
            stuffManager.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);
        }

        RemoveQuest(selected.MyQuestScript);
    }

    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        quests.Remove(qs.MyQuest);
        questDescription.text = string.Empty;
        selected = null;
        currentCount--;
        QuestCountTxt.text = currentCount + "/" + maxCount;
        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
        qs = null;
    }

    public bool HasQuest(Quest quest)
    {
        return quests.Exists(x => x.MyTitle == quest.MyTitle);
    }
}