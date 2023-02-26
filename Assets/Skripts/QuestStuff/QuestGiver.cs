using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField] private Quest[] quests;

    public Quest[] MyQuests { get => quests; }

    [SerializeField] private Sprite question, questionSilver, exclamation;

    [SerializeField] private SpriteRenderer statusRenderer;


    private void Start()
    {
        foreach (Quest quest in quests)
        {
            quest.MyQuestGiver = this;
            quest.ConnectObjectives();
        }
    }

    public void UpdateQuestStatus(QuestLog localQuestLog)
    {
        int count = 0;

        foreach (Quest quest in quests)
        {
            if (quest != null)
            {
                if (quest.IsComplete && localQuestLog.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    break;
                }
                else if (!localQuestLog.HasQuest(quest))
                {
                    statusRenderer.sprite = exclamation;
                    break;
                }
                else if (!quest.IsComplete && localQuestLog.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSilver;
                }
            }
            else
            {
                count++;

                if (count == quests.Length)
                {
                    statusRenderer.sprite = null;
                }
            }
        }
    }
}
