using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public PlayerData MyPlayerData { get; set; }

    public StorageChestData MyChestData { get; set; }

    public InventoryData MyInventoryData { get; set; }

    public List<EquipmentData> MyEquipmentData { get; set; }

    public List<ActionButtonData> MyActionButtonData { get; set; }

    public List<TalentTreeData> MyTalenTreeData { get; set; }

    //public QuestProgress MyPlayerData { get; set; }

    public SaveData()
    {
        MyInventoryData = new InventoryData();
        MyEquipmentData = new List<EquipmentData>();
        MyActionButtonData = new List<ActionButtonData>();
        MyTalenTreeData = new List<TalentTreeData>();
    }
}

[Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }
    public int MyCurrentXP { get; set; }
    public int MyPlayerGold { get; set; }
    public string MyMainClassName { get; set; }
    public string MyLefSubClassName { get; set; }
    public string MyRightSubClassName { get; set; }
    public float MyX { get; set; }
    public float MyY { get; set; }


    public PlayerData(int level, int currentXP, int playerGold, Vector2 position, string mainClassName, string leftSubClassName, string rightSubClassName)
    {
        this.MyLevel = level;
        this.MyCurrentXP = currentXP;
        this.MyPlayerGold = playerGold;
        this.MyMainClassName = mainClassName;
        this.MyLefSubClassName = leftSubClassName;
        this.MyRightSubClassName = rightSubClassName;
        this.MyX = position.x;
        this.MyY = position.y;

        Debug.Log("Saving PlayerDataFile " + mainClassName + "  " + leftSubClassName + "  " + rightSubClassName);
    }
}

[Serializable]
public class ItemData
{
    public string MyTitle { get; set; }
    public int MyStackCount { get; set; }
    public int MySlotIndex { get; set; }

    public ItemData(string title, int stackCount = 0, int slotIndex = 0)
    {
        MyTitle = title;
        MyStackCount = stackCount;
        MySlotIndex = slotIndex;
    }
}

[Serializable]
public class StorageChestData
{
    public string MyName { get; set; }
    public List<ItemData> MyItems { get; set; }

    public StorageChestData(string name)
    {
        MyName = name;
        MyItems = new List<ItemData>();
    }
}

[Serializable]
public class InventoryData
{
    public List<BagData> MyBags { get; set; }

    public InventoryData()
    {
        MyBags = new List<BagData>();
    }
}

[Serializable]
public class BagData
{
    public int MySlotCount { get; set; }
    public int MyBagIndex { get; set; }

    public BagData(int count, int index)
    {
        MySlotCount = count;
        MyBagIndex = index;
    }
}

[Serializable]
public class EquipmentData
{
    public string MyTitle { get; set; }

    public string MyType { get; set; }

    public EquipmentData(string title, string type)
    {
        MyTitle = title;
        MyType = type;
    }
}

[Serializable]
public class ActionButtonData
{
    public string MyAction { get; set; }

    public int MyIndex { get; set; }

    public ActionButtonData(string action, int index)
    {
        this.MyAction = action;
        this.MyIndex = index;
    }
}


[Serializable]
public class TalentTreeData
{
    public int myCount { get; set; }

    public TalentTreeData(int curCount)
    {
        this.myCount = curCount;
    }
}

[Serializable]
public class QuestProgress
{
    public string[] my;
}