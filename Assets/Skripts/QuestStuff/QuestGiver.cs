using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField] private Quest[] quests;

    public Quest[] MyQuests { get => quests; }




    //Debugging
    //[SerializeField] private QuestLog tmpLog;

    //private void Start()
    //{
    //    //Debugging only
    //    Transform vanc = InventoryScript.MyInstance.transform.parent.parent;
    //    tmpLog = vanc.Find("CanvasQuestUI").Find("QuestLog").GetComponent<QuestLog>();
    //    tmpLog.AcceptQuest(quests[0]);
    //    tmpLog.AcceptQuest(quests[1]);
    //}
}
