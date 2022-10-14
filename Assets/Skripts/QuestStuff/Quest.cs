using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] private string title;
    [SerializeField] private string description;

    [SerializeField] private CollectObjective[] collectObjectives;
    [SerializeField] private KillObjective[] killObjectives;

    public QuestScript MyQuestScript { get; set; }

    public string MyTitle { get => title; set => title = value; }
    public string MyDescription 
    {        
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
}

    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }

    public KillObjective[] MyKillObjectives { get => killObjectives; }

    public bool IsComplete
    {
        get
        {
            foreach (Objective o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }

            foreach (Objective o in MyKillObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }

            return true;
        }
    }

    
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField] private int amount;
    [SerializeField] private int currentAmount;
    [SerializeField] private string type;

    public int MyAmount { get => amount; }
    public int MyCurrentAmount { get => currentAmount; set => currentAmount = value; }
    public string MyType { get => type; }

    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (MyType.ToLower() == item.name.ToLower())
        {
            MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(item.name);

            if (MyCurrentAmount <= MyAmount)
            {
                QuestLog.MyInstance.transform.parent.parent.parent.GetComponent<StuffManagerScript>().WriteMessage(string.Format("{0}: {1} / {2}", item.name, MyCurrentAmount, MyAmount));
            }            

            Debug.Log(MyCurrentAmount);
            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion();
        }
    }

    public void UpdateItemCount()
    {
        MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(MyType);
        QuestLog.MyInstance.UpdateSelected();
        QuestLog.MyInstance.CheckCompletion();
    }

    public void Complete()
    {
        Stack<Item> items = InventoryScript.MyInstance.GetItems(MyType, MyAmount);

        foreach (Item item in items)
        {
            item.Remove();
        }
    }
}


[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(CharacterStats character)
    {
        if (MyType == character.MyType)
        {
            MyCurrentAmount++;
            if (MyCurrentAmount <= MyAmount)
            {
                QuestLog.MyInstance.transform.parent.parent.parent.GetComponent<StuffManagerScript>().WriteMessage(string.Format("{0}: {1} / {2}", character.MyType, MyCurrentAmount, MyAmount));
            }
            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion();
        }
    }
}