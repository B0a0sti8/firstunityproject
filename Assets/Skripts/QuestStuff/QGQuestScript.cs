using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QGQuestScript : MonoBehaviour
{
    public Quest MyQuest { get; set; }

    public void Select()
    {
        QuestGiverWindow localQuestGiverWindow = MyQuest.myQuestLog.transform.parent.Find("QuestGiverWindow").GetComponent<QuestGiverWindow>();
        localQuestGiverWindow.ShowQuestInfo(MyQuest);
    }
}
