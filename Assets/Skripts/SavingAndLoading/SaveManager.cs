using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private Item[] allItemsInTheFuckingGameBecauseTheLoadManagerNeedsToKnowWhatHeCanLoad;

    Transform PLAYER;
    Transform ownCanvases;
    PlayerStats playerStats;

    private StorageChestCanvasScript storageChest; // Falls mal mehrere storageChests existieren: private StorageChest[] storageChests;

    void Awake()
    {
        PLAYER = transform.parent;
        ownCanvases = PLAYER.Find("Own Canvases");
        playerStats = PLAYER.GetComponent<PlayerStats>();
        storageChest = ownCanvases.Find("CanvasStorageChest").Find("StorageChest").GetComponent<StorageChestCanvasScript>();
    }

    void Update()
    {
        
    }

    public void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Create);

            SaveData data = new SaveData();

            SavePlayer(data);
            SaveStorageChest(data);

            bf.Serialize(file, data);

            file.Close();


        }
        catch (System.Exception)
        {
            throw; 
            // Handling Errors
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(playerStats.MyCurrentPlayerLvl,
            playerStats.MyCurrentXP, playerStats.goldAmount, playerStats.transform.position, playerStats.className);
    }

    private void SaveStorageChest(SaveData data)
    {
        data.MyChestData = new StorageChestData(storageChest.name);

        List<Item> items = storageChest.GetItems();

        if (items.Count > 0)
        {
            foreach (Item item in items)
            {
                data.MyChestData.MyItems.Add(new ItemData(item.name, item.MySlot.MyItems.Count, item.MySlot.MyIndex));
            }
        }
    }






    public void Load()
    {

        Debug.Log("Load2");
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();

            LoadPlayer(data);
            LoadStorageChests(data);


        }
        catch (System.Exception)
        {
            // Handling Errors
            throw;
        }
    }

    public void LoadPlayer(SaveData data)
    {
        playerStats.MyCurrentPlayerLvl = data.MyPlayerData.MyLevel;                                         // Setzt geladenes Level   
        playerStats.MyCurrentXP = data.MyPlayerData.MyCurrentXP;
        playerStats.goldAmount = data.MyPlayerData.MyPlayerGold;
        playerStats.transform.position = new Vector2(data.MyPlayerData.MyX, data.MyPlayerData.MyY);         // Position des Spielers

        playerStats.LoadPlayerLevel();                                                                      // Aktualisiert Level-UI 
    }

    public void LoadStorageChests(SaveData data)
    {
        StorageChestCanvasScript chest = ownCanvases.Find("CanvasStorageChest").Find("StorageChest").GetComponent<StorageChestCanvasScript>();

        foreach (ItemData itemData in data.MyChestData.MyItems)
        {
            Item item = Array.Find(allItemsInTheFuckingGameBecauseTheLoadManagerNeedsToKnowWhatHeCanLoad, x => x.name == itemData.MyTitle);
            item.MySlot = chest.MySlots.Find(x => x.MyIndex == itemData.MySlotIndex);
            chest.AddItemThroughLoading(item, itemData.MySlotIndex);
        }
    }
}
