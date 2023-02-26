using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestScript : MonoBehaviour
{
    public Quest MyQuest { get; set; }

    private bool markedComplete = false;



    public void Select()
    {
        GetComponent<TextMeshProUGUI>().color = Color.red;
        QuestLog localQuestLog = MyQuest.myQuestLog;
        localQuestLog.ShowDescription(MyQuest);
    }

    public void DeSelect()
    {
        GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    public void IsComplete()
    {
        if (MyQuest.IsComplete && !markedComplete)
        {
            markedComplete = true;
            GetComponent<TextMeshProUGUI>().text += "(Complete)";
            QuestLog localQuestLog = MyQuest.myQuestLog;
            localQuestLog.transform.parent.parent.parent.GetComponent<StuffManagerScript>().WriteMessage(string.Format("{0} (Complete)", MyQuest.MyTitle));
        }
        else if (!MyQuest.IsComplete)
        {
            markedComplete = false;
            GetComponent<TextMeshProUGUI>().text = MyQuest.MyTitle;
        }
    }
}
