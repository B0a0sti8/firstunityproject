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

    public SaveData()
    {
        MyInventoryData = new InventoryData();
        MyEquipmentData = new List<EquipmentData>();
    }
}

[Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }
    public int MyCurrentXP { get; set; }
    public int MyPlayerGold { get; set; }
    public string MyClassName { get; set; }
    public float MyX { get; set; }
    public float MyY { get; set; }


    public PlayerData(int level, int currentXP, int playerGold, Vector2 position, string className)
    {
        this.MyLevel = level;
        this.MyCurrentXP = currentXP;
        this.MyPlayerGold = playerGold;
        this.MyClassName = className;
        this.MyX = position.x;
        this.MyY = position.y;
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