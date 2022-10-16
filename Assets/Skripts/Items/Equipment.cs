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
    [SerializeField] public int movementSpeed;		    	// Lässt Charakter schneller laufen
    [SerializeField] public int actionSpeed;				// Lässt Charakter schneller casten und angreifen. Niedrige Cooldowns, GCD
    [SerializeField] public int critChance;			   	    // Chance auf 1.5-fachen Schaden
    [SerializeField] public int critMultiplier;             // Erhöht kritischen Schaden von 1.5 auf mehr.
    [SerializeField] public int evadeChance;			 	// Lässt Charakter Schaden vermeiden
    [SerializeField] public int healInc;                    // Erhöht verursachte Heilung
    [SerializeField] public int dmgInc;                     // Erhöht verursachten Schaden
    [SerializeField] public int physRed;                    // Verringert erlittenen physischen Schaden
    [SerializeField] public int magRed;                     // Verringert erlittenen magischen Schaden
    [SerializeField] public int incHealInc;                 // Erhöht erhaltene Heilung
    [SerializeField] public int blockChance;                // Erhöht Blockchance (nur bei Schildträgern und ggf. Dual wielding)
    [SerializeField] public int skillRadInc;                // Höherer Skillradius
    [SerializeField] public int skillDurInc;                // Höhere Skilldauer
    [SerializeField] public int buffInc;                    // Stärkerer Buffeffekt
    [SerializeField] public int debuffInc;                  // Stärkerer Debuffeffekt
    [SerializeField] public int tickRateMod;                // Schnellere Tickrate für Effekte
    [SerializeField] public int lifesteal;                  // Heilt durch verursachten Schaden bzw. Heilung


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
