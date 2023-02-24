using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestGiverWindow : UIWindowNPC
{
    #region Singleton
    private static QuestGiverWindow instance;

    public static QuestGiverWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestGiverWindow>();
            }
            return instance;
        }
    }
    #endregion

    private QuestGiver questGiver;

    [SerializeField] private GameObject backBtn, acceptBtn, completeBtn;

    [SerializeField] private Transform questArea;

    [SerializeField] private GameObject questPrefab;
    [SerializeField] private GameObject questDescription;

    InventoryScript myInventory;

    private StuffManagerScript stuffManager;
    private PlayerStats playerStats;
    private GameObject PLAYER;

    private List<GameObject> quests = new List<GameObject>();

    private Quest selectedQuest;

    public void Start()
    {
        PLAYER = transform.parent.parent.parent.gameObject;
        stuffManager = PLAYER.GetComponent<StuffManagerScript>();
        playerStats = PLAYER.GetComponent<PlayerStats>();
        myInventory = PLAYER.transform.Find("Own Canvases").Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>();

    }

    public void ShowQuests(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach (GameObject go in quests)
        {
            Destroy(go);
        }

        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);

        foreach (Quest quest in questGiver.MyQuests)
        {
            if (quest != null)
            {
                GameObject go = Instantiate(questPrefab, questArea);
                go.GetComponent<TextMeshProUGUI>().text = quest.MyTitle;

                go.GetComponent<QGQuestScript>().MyQuest = quest;

                quests.Add(go);

                if (QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
                {
                    go.GetComponent<TextMeshProUGUI>().text += "(Complete)";
                }
                else if (QuestLog.MyInstance.HasQuest(quest))
                {
                    Color c = go.GetComponent<TextMeshProUGUI>().color;
                    c.a = 0.5f;
                    go.GetComponent<TextMeshProUGUI>().color = c;
                }
            }
        }
    }

    public override void Open(NPC npc)
    {
        ShowQuests((npc as QuestGiver));
        base.Open(npc);
        backBtn.SetActive(false);
        acceptBtn.SetActive(false);

    }

    public void ShowQuestInfo(Quest quest)
    {
        this.selectedQuest = quest;

        if (QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
        {
            acceptBtn.SetActive(false);
            completeBtn.SetActive(true);
        }
        else if (!QuestLog.MyInstance.HasQuest(quest))
        {
            acceptBtn.SetActive(true);
        }

        backBtn.SetActive(true);

        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);

        string objectives = string.Empty;

        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
        }

        
        questDescription.GetComponent<Text>().text = string.Format("<b><size=22>{0}</size></b>\n\n<size=14>{1}</size>\n", quest.MyTitle, quest.MyDescription); // , objectives \n\n<size=18>Objectives</size>\n<size=14>{2}</size>
    }

    public void Back()
    {
        backBtn.SetActive(false);
        acceptBtn.SetActive(false);
        ShowQuests(questGiver);
        completeBtn.SetActive(false);
    }

    public void Accept()
    {
        QuestLog.MyInstance.AcceptQuest(selectedQuest);
        Back();
    }

    public override void Close()
    {
        completeBtn.SetActive(false);
        base.Close();
    }

    public void CompleteQuest()
    {
        if (selectedQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.MyQuests.Length; i++)
            {
                if (selectedQuest == questGiver.MyQuests[i])
                {
                    questGiver.MyQuests[i] = null;
                }
            }

            foreach (CollectObjective o in selectedQuest.MyCollectObjectives)
            {
                myInventory.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
                o.Complete();
            }

            foreach (KillObjective o in selectedQuest.MyKillObjectives)
            {
                stuffManager.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);
            }

            playerStats.GainXP(selectedQuest.MyXPGiven);
            QuestLog.MyInstance.RemoveQuest(selectedQuest.MyQuestScript);
            Back();
        }
    }
}
