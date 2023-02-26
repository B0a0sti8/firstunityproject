using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Für Text-Modifikationen siehe https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html

public class CharacterPanelScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private CharPanelButtonScript helmet, shoulders, chest, cape, gloves, legs, boots, artefact, mainhand, offhand;

    public CharPanelButtonScript[] allEquipmentSlots;

    public CharPanelButtonScript MySelectedButton
    {
        get;
        set;
    }


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        allEquipmentSlots = transform.GetComponentsInChildren<CharPanelButtonScript>();
    }

    #region Stats
    Transform PLAYER;
    PlayerStats PlayerStats;

    public TextMeshProUGUI tmpArmor;

    TextMeshProUGUI tmpMastery;
    TextMeshProUGUI tmpDmgInc;
    TextMeshProUGUI tmpHealInc;

    TextMeshProUGUI tmpToughness;
    TextMeshProUGUI tmpPhysRed;
    TextMeshProUGUI tmpMagRed;
    TextMeshProUGUI tmpIncHealInc;
    TextMeshProUGUI tmpBlockChance;

    TextMeshProUGUI tmpIntellect;
    TextMeshProUGUI tmpSkillRadInc;
    TextMeshProUGUI tmpSkillDurInc;
    TextMeshProUGUI tmpCritChance;
    TextMeshProUGUI tmpCritMult;

    TextMeshProUGUI tmpCharisma;
    TextMeshProUGUI tmpBuffInc;
    TextMeshProUGUI tmpDebuffInc;

    TextMeshProUGUI tmpTempo;
    TextMeshProUGUI tmpTickRateMod;
    TextMeshProUGUI tmpActionSpeed;
    TextMeshProUGUI tmpEvade;

    TextMeshProUGUI tmpLifeSteal;
    TextMeshProUGUI tmpMoveSpeed;
    #endregion

    void Start()
    {
        PLAYER = transform.parent.parent.parent;
        PlayerStats = PLAYER.GetComponent<PlayerStats>();

        tmpArmor = transform.Find("Stats").transform.Find("Armor").GetComponent<TextMeshProUGUI>();

        tmpMastery = transform.Find("Stats").transform.Find("Mastery").GetComponent<TextMeshProUGUI>();
        tmpDmgInc = transform.Find("Stats").transform.Find("DmgInc").GetComponent<TextMeshProUGUI>();
        tmpHealInc = transform.Find("Stats").transform.Find("HealInc").GetComponent<TextMeshProUGUI>();

        tmpToughness = transform.Find("Stats").transform.Find("Toughness").GetComponent<TextMeshProUGUI>();
        tmpPhysRed = transform.Find("Stats").transform.Find("PhysRed").GetComponent<TextMeshProUGUI>();
        tmpMagRed = transform.Find("Stats").transform.Find("MagRed").GetComponent<TextMeshProUGUI>();
        tmpIncHealInc = transform.Find("Stats").transform.Find("IncHealInc").GetComponent<TextMeshProUGUI>();
        tmpBlockChance = transform.Find("Stats").transform.Find("BlockChance").GetComponent<TextMeshProUGUI>();

        tmpIntellect = transform.Find("Stats").transform.Find("Intellect").GetComponent<TextMeshProUGUI>();
        tmpSkillRadInc = transform.Find("Stats").transform.Find("SkillRadInc").GetComponent<TextMeshProUGUI>();
        tmpSkillDurInc = transform.Find("Stats").transform.Find("SkillDurInc").GetComponent<TextMeshProUGUI>();
        tmpCritChance = transform.Find("Stats").transform.Find("CritChance").GetComponent<TextMeshProUGUI>();
        tmpCritMult = transform.Find("Stats").transform.Find("CritMult").GetComponent<TextMeshProUGUI>();

        tmpCharisma = transform.Find("Stats").transform.Find("Charisma").GetComponent<TextMeshProUGUI>();
        tmpBuffInc = transform.Find("Stats").transform.Find("BuffInc").GetComponent<TextMeshProUGUI>();
        tmpDebuffInc = transform.Find("Stats").transform.Find("DebuffInc").GetComponent<TextMeshProUGUI>();

        tmpTempo = transform.Find("Stats").transform.Find("Tempo").GetComponent<TextMeshProUGUI>();
        tmpTickRateMod = transform.Find("Stats").transform.Find("TickRateMod").GetComponent<TextMeshProUGUI>();
        tmpActionSpeed = transform.Find("Stats").transform.Find("ActionSpeed").GetComponent<TextMeshProUGUI>();
        tmpEvade = transform.Find("Stats").transform.Find("Evade").GetComponent<TextMeshProUGUI>();

        tmpLifeSteal = transform.Find("Stats").transform.Find("LifeSteal").GetComponent<TextMeshProUGUI>();
        tmpMoveSpeed = transform.Find("Stats").transform.Find("MoveSpeed").GetComponent<TextMeshProUGUI>();
    }

    public void StatsUpdate()
    {
        tmpArmor.SetText("Armor: " + PlayerStats.armor.GetValue().ToString());

        tmpMastery.SetText("Mastery: " + PlayerStats.mastery.GetValue().ToString());
        tmpDmgInc.SetText("DmgInc: " + PlayerStats.dmgInc.GetValue().ToString());
        tmpHealInc.SetText("HealInc: " + PlayerStats.healInc.GetValue().ToString());

        tmpToughness.SetText("Toughness: " + PlayerStats.toughness.GetValue().ToString());
        tmpPhysRed.SetText("PhysRed: " + PlayerStats.physRed.GetValue().ToString());
        tmpMagRed.SetText("MagRed: " + PlayerStats.magRed.GetValue().ToString());
        tmpIncHealInc.SetText("IncHealInc: " + PlayerStats.incHealInc.GetValue().ToString());
        tmpBlockChance.SetText("BlockChance: " + PlayerStats.blockChance.GetValue().ToString());

        tmpIntellect.SetText("Intellect: " + PlayerStats.intellect.GetValue().ToString());
        tmpSkillRadInc.SetText("SkillRadInc: " + PlayerStats.skillRadInc.GetValue().ToString());
        tmpSkillDurInc.SetText("SkillDurInc: " + PlayerStats.skillDurInc.GetValue().ToString());
        tmpCritChance.SetText("CritChance: " + PlayerStats.critChance.GetValue().ToString());
        tmpCritMult.SetText("CritMult: " + PlayerStats.critMultiplier.GetValue().ToString());

        tmpCharisma.SetText("Charisma: " + PlayerStats.charisma.GetValue().ToString());
        tmpBuffInc.SetText("BuffInc: " + PlayerStats.buffInc.GetValue().ToString());
        tmpDebuffInc.SetText("DebuffInc: " + PlayerStats.debuffInc.GetValue().ToString());

        tmpTempo.SetText("Tempo: " + PlayerStats.tempo.GetValue().ToString());
        tmpTickRateMod.SetText("TickRateMod: " + PlayerStats.tickRateMod.GetValue().ToString());
        tmpActionSpeed.SetText("ActionSpeed: " + PlayerStats.actionSpeed.GetValue().ToString());
        tmpEvade.SetText("Evade: " + PlayerStats.evadeChance.GetValue().ToString());

        tmpLifeSteal.SetText("LifeSteal: " + PlayerStats.lifesteal.GetValue().ToString());
        tmpMoveSpeed.SetText("MoveSpeed: " + PlayerStats.movementSpeed.GetValue().ToString());
    }

    #region TooltipStats
    public string ttArmor;

    public string ttMastery;
    public string ttDmgInc;
    public string ttHealInc;

    public string ttToughness;
    public string ttPhysRed;
    public string ttMagRed;
    public string ttIncHealInc;
    public string ttBlockChance;

    public string ttIntellect;
    public string ttSkillRadInc;
    public string ttSkillDurInc;
    public string ttCritChance;
    public string ttCritMult;

    public string ttCharisma;
    public string ttBuffInc;
    public string ttDebuffInc;

    public string ttTempo;
    public string ttTickRateMod;
    public string ttActionSpeed;
    public string ttEvade;

    public string ttLifeSteal;
    public string ttMoveSpeed;
    #endregion

    public void UpdateStatTooltip()
    {
        float tempArmor;
        tempArmor = PlayerStats.armor.GetValue() - PlayerStats.armor.baseValue;
        if (tempArmor == 0)
        {
            ttArmor = "<color=#00ffffff>Armor:</color>\n" +
            "More <color=grey>Armor</color> :3\n" +
            "\n" +
            "You have <color=yellow><b>" + PlayerStats.armor.GetValue().ToString() + "</b></color> <color=grey>Armor</color>!";
        } else
        {
            ttArmor = "<color=#00ffffff>Armor:</color>\n" +
            "More <color=grey>Armor</color> :3\n" +
            "\n" +
            "You have <color=yellow><b>" + PlayerStats.armor.GetValue().ToString() + "</b></color> <color=grey>Armor</color>!\n" +
            "\n" +
            "           Base Armor: " + PlayerStats.armor.baseValue.ToString() + "\n" +
            "Temporary Armor: <color=#ff00ffff>" + tempArmor + "</color>\n" +
            "-----> Total Armor: " + PlayerStats.armor.GetValue();
        }
       

        ttMastery = "<color=#00ffffff>Mastery:</color>\n" +
            "Increases <color=orange>DmgInc</color> and <color=orange>HealInc</color>";

        ttDmgInc = "<color=#00ffffff>Damage Increase:</color>\n" +
            "Increases your damage by " + PlayerStats.dmgInc.GetValue().ToString();

        ttHealInc = "<color=#00ffffff>Heal Increase:</color>\n" +
            "Increases your healing by " + PlayerStats.healInc.GetValue().ToString();


        ttToughness = "<color=#00ffffff>Toughness:</color>\n" +
            "Increases your <color=red>Health</color>\n" +
            "Increases <color=orange>PhysRed</color>, <color=orange>MagRed</color>, " +
            "<color=orange>IncHealInc</color> and <color=orange>BlockChance</color>";

        ttPhysRed = "<color=#00ffffff>Physical Reduction:</color>\n" +
            "Increases your physical damage reduction";

        ttMagRed = "<color=#00ffffff>Magical Reduction:</color>\n" +
            "Increases your magical damage reduction";

        ttIncHealInc = "<color=#00ffffff>Incoming Healing Increase:</color>\n" +
            "<size=30>Increases</size> your <b>incoming</b> <i>healing</i>";

        ttBlockChance = "<color=#00ffffff>Block Chance:</color>\n" +
            "Increases your chance <color=grey>Block</color> a hit";


        ttIntellect = "<color=#00ffffff>Intellect:</color>\n" +
            "Increases your Maximum <color=blue>Mana</color>\n" +
            "Increases <color=orange>SkillRadInc</color>, <color=orange>SkillDurInc</color>, " +
            "<color=orange>CritChance</color> and <color=orange>CritMult</color>";

        ttSkillRadInc = "<color=#00ffffff>Skill Radius Increase:</color>\n";

        ttSkillDurInc = "<color=#00ffffff>Skill Duration Increase:</color>\n";

        ttCritChance = "<color=#00ffffff>Crit Chance:</color>\n" +
            "<color=yellow>" + PlayerStats.critChance.GetValue().ToString() + "</color>% Chance to get a Critical Hit";

        ttCritMult = "<color=#00ffffff>Crit Multiplier:</color>\n" +
            "Critical Hits do <color=yellow>" + PlayerStats.critMultiplier.GetValue().ToString() + "</color>% (more) damage";


        ttCharisma = "<color=#00ffffff>Charisma:</color>\n" +
            "Increases <color=orange>BuffInc</color> and <color=orange>DebuffInc</color>";

        ttBuffInc = "<color=#00ffffff>Buff Increase:</color>\n" +
            "Makes your Buffs better";

        ttDebuffInc = "<color=#00ffffff>DebuffIncrease:</color>\n" +
            "Makes your Debuffs better";


        ttTempo = "<color=#00ffffff>Tempo:</color>\n" +
            "Increases <color=orange>TickRateMod</color>, <color=orange>ActionSpeed</color> and <color=orange>Evade</color>";

        ttTickRateMod = "<color=#00ffffff>Tick Rate Modifier:</color>\n";

        ttActionSpeed = "<color=#00ffffff>Action Speed:</color>\n";

        ttEvade = "<color=#00ffffff>Evade Chance:</color>\n" +
            "Increases the Chance to Doge an attack (no damage)";


        ttLifeSteal = "<color=#00ffffff>Life Steal:</color>\n" +
            "";

        ttMoveSpeed = "<color=#00ffffff>Movement Speed:</color>\n" +
            "Increases the <color=#00ff00ff><i>Movement Speed</i></color> of the Character";
    }

    void Update()
    {
        StatsUpdate(); //!!! anders implementieren !!!
        UpdateStatTooltip();
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha <= 0)
        {
            //StatsUpdate();
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }

    public void EquipStuff(Equipment equipment)
    {
        switch (equipment.MyEquipmentType)
        {
            case EquipmentType.Helmet:
                helmet.EquipStuff(equipment);
                break;
            case EquipmentType.Shoulders:
                shoulders.EquipStuff(equipment);
                break;
            case EquipmentType.Chest:
                chest.EquipStuff(equipment);
                break;
            case EquipmentType.Cape:
                cape.EquipStuff(equipment);
                break;
            case EquipmentType.Gloves:
                gloves.EquipStuff(equipment);
                break;
            case EquipmentType.Legs:
                legs.EquipStuff(equipment);
                break;
            case EquipmentType.Boots:
                boots.EquipStuff(equipment);
                break;
            case EquipmentType.Artefact:
                artefact.EquipStuff(equipment);
                break;
            case EquipmentType.Mainhand:
                mainhand.EquipStuff(equipment);
                break;
            case EquipmentType.OffHand:
                offhand.EquipStuff(equipment);
                break;
            case EquipmentType.TwoHand:
                mainhand.EquipStuff(equipment);
                break;
        }
    }
}
