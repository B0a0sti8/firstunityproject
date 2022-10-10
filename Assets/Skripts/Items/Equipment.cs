using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EquipmentType { Helmet, Shoulders, Chest, Cape, Gloves, Legs, Boots, Artefact, Mainhand, OffHand, TwoHand }
enum EquipmentClass { Light, Medium, Heavy }



[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment", order = 2)]

public class Equipment : Item
{
    [SerializeField] private EquipmentType equpipType;
    [SerializeField] private EquipmentClass equipClass;



    // Hauptstats
    [SerializeField] public int armor = 0;           // 
    [SerializeField] public int weaponDamage = 0;    // 
    [SerializeField] public int mastery = 0;         // 
    [SerializeField] public int toughness = 0;       // 
    [SerializeField] public int intellect = 0;       // 
    [SerializeField] public int charisma = 0;        // 
    [SerializeField] public int tempo = 0;           // 

    // Nebenstats
    //[SerializeField] private int castspeed = 0;
    //[SerializeField] private int evade = 0;
    //[SerializeField] private int health = 0;
    //[SerializeField] private int mana = 0;
    //[SerializeField] private int crit = 0;

    private static Dictionary<ItemQuality, string> nameColors = new Dictionary<ItemQuality, string>()
    {{ ItemQuality.Common, "#ECEBEB8A"},{ ItemQuality.Uncommon, "#05CB198A"},{ ItemQuality.Rare, "#141FC38A"},{ ItemQuality.Epic, "#B512A78A"},{ ItemQuality.Mythic, "#FDB9478A"}};

    private static Dictionary<ItemQuality, string> MyNameColors
    {
        get
        {
            return nameColors;
        }
    }

    internal EquipmentType MyEquipmentType { get => equpipType; }

    public override void Awake()
    {
        base.Awake();
        tooltipItemName = GetItemTooltipName();
        tooltipItemDescription = GetItemDescription();
    }

    public override void Use()
    {
        base.Use();
        Equip();
    }

    public string GetItemDescription()
    {
        string tooltipStats = "";

        if (armor != 0)
        { tooltipStats += string.Format("\n +{0} armor", armor); }

        if (mastery != 0)
        { tooltipStats += string.Format("\n +{0} mastery", mastery); }

        if (toughness != 0)
        { tooltipStats += string.Format("\n +{0} toughness", toughness); }

        if (intellect != 0)
        { tooltipStats += string.Format("\n +{0} confidence", intellect); }


        string Lore = "";
        string Cost = "";


        string description = tooltipStats + Lore + Cost;
        return description;
    }

    public string GetItemTooltipName()
    {
        string tmpName = string.Format("<color={0}>{1}</color>", MyNameColors[itemQuality], name);

        return tmpName;
    }

    public void Equip()
    {
        CharacterPanelScript.MyInstance.EquipStuff(this);
    }
}
