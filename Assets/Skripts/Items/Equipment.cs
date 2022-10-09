using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EquipmentType { Helmet, Chest, Gloves, Boots, Legs, Shoulders, Cape, Mainhand, OffHand, TwoHand, Artefact }
enum EquipmentClass { Light, Medium, Heavy }



[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment", order = 2)]

public class Equipment : Item
{
    [SerializeField] private EquipmentType equpipType;
    [SerializeField] private EquipmentClass equipClass;

    [SerializeField] private int armor = 0;          // Gibt Schutz

    // Hauptstats
    [SerializeField] private int mastery = 0;        // Mehr Cast- und Movementspeed
    [SerializeField] private int toughness = 0;      // Leben, Block, Evade, Resistenz
    [SerializeField] private int confidence = 0;     // Krit, Mana

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

    public override void Awake()
    {
        base.Awake();
        tooltipItemName = GetItemTooltipName();
        tooltipItemDescription = GetItemDescription();
    }


    public string GetItemDescription()
    {
        string tooltipStats = "";

        if (armor > 0)
        { tooltipStats += string.Format("\n +{0} armor", armor); }

        if (mastery > 0)
        { tooltipStats += string.Format("\n +{0} mastery", mastery); }

        if (toughness > 0)
        { tooltipStats += string.Format("\n +{0} toughness", toughness); }

        if (confidence > 0)
        { tooltipStats += string.Format("\n +{0} confidence", confidence); }


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
}
