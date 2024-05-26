using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Netcode;
using UnityEngine;

public class PlayerSaveLoad : MonoBehaviour
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
    Transform talentTree;
    ClassAssignment classAssignment;
    SkillbookMaster mySkillBookMaster;


    void Awake()
    {
        PLAYER = transform.parent;
        ownCanvases = PLAYER.Find("Own Canvases");
        playerStats = PLAYER.GetComponent<PlayerStats>();
        storageChest = ownCanvases.Find("CanvasStorageChest").Find("StorageChest").GetComponent<StorageChestCanvasScript>();
        inventoryScript = ownCanvases.Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>();
        characterPanel = ownCanvases.Find("CanvasCharacterPanel").Find("CharacterPanel").GetComponent<CharacterPanelScript>();
        actionButtons = ownCanvases.Find("Canvas Action Skills").Find("SkillSlots").GetComponentsInChildren<ActionButton>();
        talentTree = ownCanvases.Find("CanvasTalentTree");
        classAssignment = ownCanvases.Find("CanvasClassChoice").GetComponent<ClassAssignment>();
        mySkillBookMaster = ownCanvases.Find("Canvas Skillbook").GetComponent<SkillbookMaster>();

    }


    public void Save(string characterName)
    {
        string characterLevel = playerStats.MyCurrentPlayerLvl.ToString();
        string mainQuest = "HierMainquestverweis"; // Sobald Questlog mit eingebaut, hier ablegen

        Debug.Log("Ich versuche einfach mal zu speichern... :)");
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveFile_" + characterName + "_" + characterLevel + "_" + mainQuest + "_" + ".dat", FileMode.Create);

            SaveData data = new SaveData();


            SavePlayer(data);
            SaveTalents(data);
            SaveTalentTreeRingRotation(data);
            //SaveEquipment(data);
            //SaveBags(data);
            //SaveStorageChest(data);
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
        Debug.Log("Saving Player.");
        Debug.Log(playerStats.mainClassName + "  " + playerStats.leftSubClassName + "  " + playerStats.rightSubClassName);
        data.MyPlayerData = new PlayerData(playerStats.MyCurrentPlayerLvl,
            playerStats.MyCurrentXP, playerStats.goldAmount, playerStats.transform.position, playerStats.mainClassName, playerStats.leftSubClassName, playerStats.rightSubClassName);
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

    private void SaveBags(SaveData data)
    {
        for (int i = 1; i < inventoryScript.MyBags.Count; i++)
        {
            data.MyInventoryData.MyBags.Add(new BagData(inventoryScript.MyBags[i].MySlotCount, inventoryScript.MyBags[i].MyBagButton.MyBagIndex));
        }
    }

    private void SaveEquipment(SaveData data)
    {
        foreach (CharPanelButtonScript charButton in characterPanel.allEquipmentSlots)
        {
            if (charButton.MyEquip != null)
            {
                data.MyEquipmentData.Add(new EquipmentData(charButton.MyEquip.name, charButton.name));
            }
        }
    }

    private void SaveActionButtons(SaveData data)
    {
        ActionButtonData a;
        for (int i = 0; i < actionButtons.Length; i++)
        {
            a = new ActionButtonData(actionButtons[i].skillName, i);
            data.MyActionButtonData.Add(a);
        }
    }

    private void SaveTalents(SaveData data)
    {
        TalentTreeData tData;
        TalentTree myTaTree = talentTree.GetComponent<TalentTree>();
        foreach (Talent talent in myTaTree.GetTalentsForSaving())
        {
            tData = new TalentTreeData(talent.currentCount);
            data.MyTalenTreeData.Add(tData);
        }
        Debug.Log(data.MyTalenTreeData.Count);
    }

    private void SaveTalentTreeRingRotation(SaveData data)
    {
        TalentTreeRingRotationData tRotData;
        TalentTree myTaTree = talentTree.GetComponent<TalentTree>();
        for (int i = 0; i < 4; i++)
        {
            Transform detunedRing = myTaTree.transform.Find("TalentTreeWindow").Find("MainBody").Find("MaskLayer").Find("TalentTree").Find("Tier" + (i + 1).ToString());
            float myRotation = detunedRing.rotation.eulerAngles.z;
            tRotData = new TalentTreeRingRotationData(myRotation);
            data.MyTalentTreeRotationData.Add(tRotData);
        }

        Debug.Log(data.MyTalentTreeRotationData.Count);
    }

    private string FindRightFileToLoad(string charName)
    {
        string myRightFileName = "";

        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles();

        foreach (FileInfo fInfo in fileInfo)
        {
            string[] ident = fInfo.Name.Split("_");
            if (ident[0] == "SaveFile")
            {
                //Debug.Log(fInfo.Name);
                string characterName = ident[1];

                if (characterName == charName) { myRightFileName = fInfo.Name; break; }
            }
        }
        return myRightFileName;
    }

    public void Load(string characterName)
    {
        if (!PLAYER.GetComponent<NetworkObject>().IsOwner) return;

        Debug.Log("Loading Character... :" + characterName);
        string myFileName = FindRightFileToLoad(characterName);
        if (myFileName == "") { Debug.Log("No File Found! "); return; }

        SaveData data = new SaveData();

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + myFileName, FileMode.Open);
            data = (SaveData)bf.Deserialize(file);
            file.Close();
        }
        catch (System.Exception)
        {
            // Handling Errors
            throw;
        }

        LoadPlayer(data);
        classAssignment.ChangeAndSetClass("main", playerStats.mainClassName);
        classAssignment.ChangeAndSetClass("left", playerStats.leftSubClassName);
        classAssignment.ChangeAndSetClass("right", playerStats.rightSubClassName);

        talentTree.GetComponent<TalentTree>().ResetSkillTree();
        StartCoroutine(ContinueLoading(data));


        Debug.Log("Loading Character...  player classes loaded.");


        //LoadEquipment(data);
        //LoadBags(data);
        //LoadStorageChests(data);


    }

    IEnumerator ContinueLoading(SaveData data)
    {
        yield return 1;
        LoadTalentTreeRingRotation(data);
        LoadTalents(data);
        LoadActionButtons(data);
    }

    private void LoadPlayer(SaveData data)
    {
        playerStats.MyCurrentPlayerLvl = data.MyPlayerData.MyLevel;                                         // Setzt geladenes Level   
        playerStats.MyCurrentXP = data.MyPlayerData.MyCurrentXP;
        playerStats.goldAmount = data.MyPlayerData.MyPlayerGold;
        playerStats.LoadPlayerLevel();// Aktualisiert Level-UI 
        //playerStats.transform.position = new Vector2(data.MyPlayerData.MyX, data.MyPlayerData.MyY);         // Position des Spielers

        // Spieler Klassen
        playerStats.mainClassName = data.MyPlayerData.MyMainClassName;
        playerStats.leftSubClassName = data.MyPlayerData.MyLefSubClassName;
        playerStats.rightSubClassName = data.MyPlayerData.MyRightSubClassName;

        Debug.Log("Loading PlayerDataFile " + data.MyPlayerData.MyMainClassName + "  " + data.MyPlayerData.MyLefSubClassName + "  " + data.MyPlayerData.MyRightSubClassName);                                                 
    }

    private void LoadTalents(SaveData data)
    {
        List<int> talentCurrentCounts = new List<int>();
        for (int i = 0; i < data.MyTalenTreeData.Count; i++) talentCurrentCounts.Add(data.MyTalenTreeData[i].myCount);
        //talentTree.gameObject.SetActive(true);
        talentTree.GetComponent<TalentTree>().AutoSkillWhenLoading(talentCurrentCounts);
        //talentTree.gameObject.SetActive(false);
    }

    private void LoadTalentTreeRingRotation(SaveData data)
    {
        List<float> myRingRotations = new List<float>();
        for (int i = 0; i < data.MyTalentTreeRotationData.Count; i++) myRingRotations.Add(data.MyTalentTreeRotationData[i].myRingRotation);
        talentTree.GetComponent<TalentTree>().SetRingRotationWhenLoading(myRingRotations);

    }

    private void LoadStorageChests(SaveData data)
    {
        StorageChestCanvasScript chest = ownCanvases.Find("CanvasStorageChest").Find("StorageChest").GetComponent<StorageChestCanvasScript>();

        foreach (ItemData itemData in data.MyChestData.MyItems)
        {
            Item item = Array.Find(allItemsInTheFuckingGameBecauseTheLoadManagerNeedsToKnowWhatHeCanLoad, x => x.name == itemData.MyTitle);
            item.MySlot = chest.MySlots.Find(x => x.MyIndex == itemData.MySlotIndex);
            chest.AddItemThroughLoading(item, itemData.MySlotIndex);
        }
    }

    private void LoadBags(SaveData data)
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

    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.MyEquipmentData)
        {
            CharPanelButtonScript cb = Array.Find(characterPanel.allEquipmentSlots, x => x.name == equipmentData.MyType);
            cb.EquipStuff(Array.Find(allItemsInTheFuckingGameBecauseTheLoadManagerNeedsToKnowWhatHeCanLoad, x => x.name == equipmentData.MyTitle) as Equipment);
        }
    }

    private void LoadActionButtons(SaveData data)
    {
        for (int i = 0; i < data.MyActionButtonData.Count; i++)
        {
            actionButtons[data.MyActionButtonData[i].MyIndex].skillName = data.MyActionButtonData[i].MyAction;
            actionButtons[data.MyActionButtonData[i].MyIndex].RemoteUpdateThisButton();
            mySkillBookMaster.UpdateCurrentSkills();
        }
    }
}