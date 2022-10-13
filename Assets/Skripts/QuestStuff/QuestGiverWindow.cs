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

    [SerializeField] private GameObject backBtn, acceptBtn;

    [SerializeField] private Transform questArea;

    [SerializeField] private GameObject questPrefab;
    [SerializeField] private GameObject questDescription;

    public void ShowQuests(QuestGiver questGiver)
    {
        this.questGiver = questGiver;
        foreach (Quest quest in questGiver.MyQuests)
        {
            GameObject go = Instantiate(questPrefab, questArea);
            go.GetComponent<TextMeshProUGUI>().text = quest.MyTitle;

            go.GetComponent<QGQuestScript>().MyQuest = quest;
        }
    }

    public override void Open(NPC npc)
    {
        ShowQuests((npc as QuestGiver));
        base.Open(npc);

    }

    public void ShowQuestInfo(Quest quest)
    {
        backBtn.SetActive(true);
        acceptBtn.SetActive(true);
        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);

        string objectives = string.Empty;

        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
        }

        
        questDescription.GetComponent<Text>().text = string.Format("<b><size=22>{0}</size></b>\n\n<size=14>{1}</size>\n\n<size=18>Objectives</size>\n<size=14>{2}</size>", quest.MyTitle, quest.MyDescription, objectives);
    }
}
