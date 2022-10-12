using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
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
        GameObject go = Instantiate(questPrefab, questParent);

        QuestScript qs = go.GetComponent<QuestScript>();
        quest.MyQuestScript = qs;
        qs.MyQuest = quest;
        
        go.GetComponent<TextMeshProUGUI>().text = quest.MyTitle;
    }

    public void ShowDescription(Quest quest)
    {
        if (selected != null)
        {
            selected.MyQuestScript.DeSelect();
        }

        selected = quest;

        string title = quest.MyTitle;
        questDescription.text = string.Format("<b>{0}</b>", title);

    }
}
