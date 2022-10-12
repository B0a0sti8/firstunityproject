using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private Quest[] quests;

    //Debugging
    [SerializeField] private QuestLog tmpLog;

    private void Start()
    {

        //Debugging only
        Debug.Log(InventoryScript.MyInstance.transform);
        Transform vanc = InventoryScript.MyInstance.transform.parent.parent;
        Debug.Log(vanc);
        tmpLog = vanc.Find("CanvasQuestUI").Find("QuestLog").GetComponent<QuestLog>();
        tmpLog.AcceptQuest(quests[0]);
        tmpLog.AcceptQuest(quests[1]);
    }

}
