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

    [SerializeField]
    private ActionButton[] actionButtons;

    Transform PLAYER;
    Transform ownCanvases;
    PlayerStats playerStats;
    StorageChestCanvasScript storageChest; // Falls mal mehrere storageChests existieren: private StorageChest[] storageChests;
    InventoryScript inventoryScript;
    CharacterPanelScript characterPanel;
    

    void Awake()
    {
        PLAYER = transform.parent;
        ownCanvases = PLAYER.Find("Own Canvases");
        playerStats = PLAYER.GetComponent<PlayerStats>();
        storageChest = ownCanvases.Find("CanvasStorageChest").Find("StorageChest").GetComponent<StorageChestCanvasScript>();
        inventoryScript = ownCanvases.Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>();
        characterPanel = ownCanvases.Find("CanvasCharacterPanel").Find("CharacterPanel").GetComponent<CharacterPanelScript>();
        actionButtons = ownCanvases.Find("Canvas Action Skills").Find("SkillSlots").GetComponentsInChildren<ActionButton>();
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

            SaveEquipment(data);
            SaveBags(data);
            SavePlayer(data);
            SaveStorageChest(data);
            SaveActionButtons(data);

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

    public void SaveBags(SaveData data)
    {
        for (int i = 1; i < inventoryScript.MyBags.Count; i++)
        {
            data.MyInventoryData.MyBags.Add(new BagData(inventoryScript.MyBags[i].MySlotCount, inventoryScript.MyBags[i].MyBagButton.MyBagIndex));
        }
    }

    public void SaveEquipment(SaveData data)
    {
        foreach (CharPanelButtonScript charButton in characterPanel.allEquipmentSlots)
        {
            if (charButton.MyEquip != null)
            {
                data.MyEquipmentData.Add(new EquipmentData(charButton.MyEquip.name, charButton.name));
            }
        }
    }

    public void SaveActionButtons(SaveData data)
    {
        ActionButtonData a;
        for (int i = 0; i < actionButtons.Length; i++)
        {
            a = new ActionButtonData(actionButtons[i].skillName, i);
            data.MyActionButtonData.Add(a);
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

            LoadEquipment(data);
            LoadBags(data);
            LoadPlayer(data);
            LoadStorageChests(data);
            LoadActionButtons(data);


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

    public void LoadBags(SaveData data)
    {
        int bagIndexInArray = 0; int ix = 0;
        foreach (Item item in allItemsInTheFuckingGameBecauseTheLoadManagerNeedsToKnowWhatHeCanLoad)
        {
            if (item is Bag)
            {
                bagIndexInArray = ix;
            }

            ix++;
        }

        foreach (BagData bagData in data.MyInventoryData.MyBags)
        {
            Bag newBag = (Bag)Instantiate(allItemsInTheFuckingGameBecauseTheLoadManagerNeedsToKnowWhatHeCanLoad[bagIndexInArray]);
            newBag.Initialize(bagData.MySlotCount);
            inventoryScript.AddBag(newBag, bagData.MyBagIndex);
        }
    }

    public void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.MyEquipmentData)
        {
            CharPanelButtonScript cb = Array.Find(characterPanel.allEquipmentSlots, x => x.name == equipmentData.MyType);
            cb.EquipStuff(Array.Find(allItemsInTheFuckingGameBecauseTheLoadManagerNeedsToKnowWhatHeCanLoad, x => x.name == equipmentData.MyTitle) as Equipment);
        }
    }

    public void LoadActionButtons(SaveData data)
    {
        for (int i = 0; i < data.MyActionButtonData.Count; i++)
        {
            actionButtons[data.MyActionButtonData[i].MyIndex].skillName = data.MyActionButtonData[i].MyAction;
        }
    }
}