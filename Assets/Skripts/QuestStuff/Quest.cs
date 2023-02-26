using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public QuestLog myQuestLog = null;

    [SerializeField] private string title;
    [SerializeField] private string description;

    [SerializeField] private int xPGiven;

    [SerializeField] private CollectObjective[] collectObjectives;
    [SerializeField] private KillObjective[] killObjectives;

    public QuestScript MyQuestScript { get; set; }

    public QuestGiver MyQuestGiver { get; set; }

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

    public int MyXPGiven { get => xPGiven; set => xPGiven = value; }

    public void ConnectObjectives()
    {
        foreach (CollectObjective cO in collectObjectives)
        {
            cO.myQuest = this;
            if (myQuestLog != null)
            {
                cO.myQuestLog = this.myQuestLog;
                cO.myInventory = this.myQuestLog.transform.parent.parent.Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>();
            }
        }

        foreach (KillObjective kO in killObjectives)
        {
            kO.myQuest = this;
            if (myQuestLog != null)
            {
                kO.myQuestLog = this.myQuestLog;
                kO.myInventory = this.myQuestLog.transform.parent.parent.Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>();
            }
        }
    }
}

[System.Serializable]
public abstract class Objective
{
    public Quest myQuest = null;
    public InventoryScript myInventory = null;
    public QuestLog myQuestLog = null;

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
            MyCurrentAmount = myInventory.GetItemCount(item.name);

            if (MyCurrentAmount <= MyAmount)
            {
                myQuestLog.transform.parent.parent.parent.GetComponent<StuffManagerScript>().WriteMessage(string.Format("{0}: {1} / {2}", item.name, MyCurrentAmount, MyAmount));
            }            

            Debug.Log(MyCurrentAmount);
            myQuestLog.UpdateSelected();
            myQuestLog.CheckCompletion();
        }
    }

    public void UpdateItemCount()
    {
        MyCurrentAmount = myInventory.GetItemCount(MyType);
        myQuestLog.UpdateSelected();
        myQuestLog.CheckCompletion();
    }

    public void Complete()
    {
        Stack<Item> items = myInventory.GetItems(MyType, MyAmount);

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
            if (MyCurrentAmount <= MyAmount)
            {
                MyCurrentAmount++;
                myQuestLog.transform.parent.parent.parent.GetComponent<StuffManagerScript>().WriteMessage(string.Format("{0}: {1} / {2}", character.MyType, MyCurrentAmount, MyAmount));
                myQuestLog.UpdateSelected();
                myQuestLog.CheckCompletion();
            }
        }
    }
}