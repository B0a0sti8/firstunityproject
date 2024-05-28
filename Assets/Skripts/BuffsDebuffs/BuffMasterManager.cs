using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMasterManager : MonoBehaviour
{
    private static BuffMasterManager instance;

    public static BuffMasterManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BuffMasterManager>();
            }
            return instance;
        }
    }

    public Dictionary<string, Buff> ListOfAllBuffs = new Dictionary<string, Buff>();
    public Dictionary<string, Sprite> ListOfAllBuffSprites = new Dictionary<string, Sprite>();

    [Header("Allgemeine Buffs")]// Für allgemeine Buffs
    [SerializeField ]Sprite hoTBuffSprite;
    [SerializeField] Sprite speedBoostBuffSprite;
    [SerializeField] Sprite attackSpeedBoostBuffSprite;
    [SerializeField] Sprite stunnedEffectOnEnemiesSprite;

    [Header("Buffs zum Testen und Spielen")]// Zum testen
    [SerializeField] Sprite testWayOfTheChickenDamageDebuffSprite;

    [Header("Summoner Buffs")]// Für Summoner-spezifische Buffs
    [SerializeField] Sprite summonInsectsOnEnemyDeathBuffSprite;
    [SerializeField] Sprite summonAstralSnakeDoTSprite;
    [SerializeField] Sprite summonAstralSnakeDebuffSprite;
    [SerializeField] Sprite summonSpiritWolfOnSkillBuffSprite;
    [SerializeField] Sprite mainMinionBuffDamageAndHealingSprite;
    [SerializeField] Sprite summonSpiderSlowEffectBuffSprite;
    [SerializeField] Sprite theGreatSacrificeBuffSprite;

    [Header("Warrior Buffs")]// Für Warrior-spezifische Buffs
    [SerializeField] Sprite warrior_StrikeComboBuff1_Sprite;
    [SerializeField] Sprite warrior_SlashComboBuff1_Sprite;
    [SerializeField] Sprite warrior_StingComboBuff1_Sprite;
    [SerializeField] Sprite warrior_StrikeComboBuff2_Sprite;
    [SerializeField] Sprite warrior_StingComboBuff2_Sprite;
    [SerializeField] Sprite warrior_SlashComboBuff2_Sprite;
    [SerializeField] Sprite warrior_OffensiveStanceBuff_Sprite;
    [SerializeField] Sprite warrior_DefensiveStanceBuff_Sprite;
    [SerializeField] Sprite warrior_TearingSlashDoT_Sprite;
    [SerializeField] Sprite warrior_DevastatingStrikeDebuff_Sprite;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Allgemeine Buffs
        HoTBuff hoTBuff = new HoTBuff();
        ListOfAllBuffs.Add("HoTBuff", hoTBuff);
        ListOfAllBuffSprites.Add("HoTBuff", hoTBuffSprite);

        SpeedBoostBuff speedBoostBuff = new SpeedBoostBuff();
        ListOfAllBuffs.Add("speedBoostBuff", speedBoostBuff);
        ListOfAllBuffSprites.Add("speedBoostBuff", speedBoostBuffSprite);

        AttackSpeedBoostBuff attackSpeedBoostBuff = new AttackSpeedBoostBuff();
        ListOfAllBuffs.Add("AttackSpeedBoostBuff", attackSpeedBoostBuff);
        ListOfAllBuffSprites.Add("AttackSpeedBoostBuff", attackSpeedBoostBuffSprite);

        StunnedEffectOnEnemies stunnedEffectOnEnemies = new StunnedEffectOnEnemies();
        ListOfAllBuffs.Add("StunnedEffectOnEnemies", stunnedEffectOnEnemies);
        ListOfAllBuffSprites.Add("StunnedEffectOnEnemies", stunnedEffectOnEnemiesSprite);

        // Buffs zum Testen 
        TheWayOfTheChickenDamageDebuff testWayOfTheChickenDamageDebuff = new TheWayOfTheChickenDamageDebuff();
        ListOfAllBuffs.Add("TestWayOfTheChickenDamageDebuff", testWayOfTheChickenDamageDebuff);
        ListOfAllBuffSprites.Add("TestWayOfTheChickenDamageDebuff", testWayOfTheChickenDamageDebuffSprite);

        // Spezifisch für Summoner-Klasse
        SummonInsectsOnEnemyDeathBuff summonInsectsOnEnemyDeathBuff = new SummonInsectsOnEnemyDeathBuff();
        ListOfAllBuffs.Add("SummonInsectsOnEnemyDeathBuff", summonInsectsOnEnemyDeathBuff);
        ListOfAllBuffSprites.Add("SummonInsectsOnEnemyDeathBuff", summonInsectsOnEnemyDeathBuffSprite);

        SummonAstralSnakeDoT summonAstralSnakeDoT = new SummonAstralSnakeDoT();
        ListOfAllBuffs.Add("SummonAstralSnakeDoT", summonAstralSnakeDoT);
        ListOfAllBuffSprites.Add("SummonAstralSnakeDoT", summonAstralSnakeDoTSprite);

        SummonAstralSnakeDebuff summonAstralSnakeDebuff = new SummonAstralSnakeDebuff();
        ListOfAllBuffs.Add("SummonAstralSnakeDebuff", summonAstralSnakeDebuff);
        ListOfAllBuffSprites.Add("SummonAstralSnakeDebuff", summonAstralSnakeDebuffSprite);

        SummonSpiritWolfOnSkillBuff summonSpiritWolfOnSkillBuff = new SummonSpiritWolfOnSkillBuff();
        ListOfAllBuffs.Add("SummonSpiritWolfOnSkillBuff", summonSpiritWolfOnSkillBuff);
        ListOfAllBuffSprites.Add("SummonSpiritWolfOnSkillBuff", summonSpiritWolfOnSkillBuffSprite);

        MainMinionBuffDamageAndHealingBuff mainMinionBuffDamageAndHealingBuff = new MainMinionBuffDamageAndHealingBuff();
        ListOfAllBuffs.Add("MainMinionBuffDamageAndHealingBuff", mainMinionBuffDamageAndHealingBuff);
        ListOfAllBuffSprites.Add("MainMinionBuffDamageAndHealingBuff", mainMinionBuffDamageAndHealingSprite);

        SummonerSpiderSlowEffectBuff summonerSpiderSlowEffecBuff = new SummonerSpiderSlowEffectBuff();
        ListOfAllBuffs.Add("SummonerSpiderSlowEffectDebuff", summonerSpiderSlowEffecBuff);
        ListOfAllBuffSprites.Add("SummonerSpiderSlowEffectDebuff", summonSpiderSlowEffectBuffSprite);

        TheGreatSacrificeBuff theGreatSacrificeBuff = new TheGreatSacrificeBuff();
        ListOfAllBuffs.Add("TheGreatSacrificeBuff", theGreatSacrificeBuff);
        ListOfAllBuffSprites.Add("TheGreatSacrificeBuff", theGreatSacrificeBuffSprite);

        // Spezifisch für Warrior-Klasse
        Warrior_StrikeComboBuff1 warrior_StrikeComboBuff1 = new Warrior_StrikeComboBuff1();
        ListOfAllBuffs.Add("Warrior_StrikeComboBuff1", warrior_StrikeComboBuff1);
        ListOfAllBuffSprites.Add("Warrior_StrikeComboBuff1", this.warrior_StrikeComboBuff1_Sprite);

        Warrior_SlashComboBuff1 warrior_SlashComboBuff1 = new Warrior_SlashComboBuff1();
        ListOfAllBuffs.Add("Warrior_SlashComboBuff1", warrior_SlashComboBuff1);
        ListOfAllBuffSprites.Add("Warrior_SlashComboBuff1", warrior_SlashComboBuff1_Sprite);

        Warrior_StingComboBuff1 warrior_StingComboBuff1 = new Warrior_StingComboBuff1();
        ListOfAllBuffs.Add("Warrior_StingComboBuff1", warrior_StingComboBuff1);
        ListOfAllBuffSprites.Add("Warrior_StingComboBuff1", warrior_StingComboBuff1_Sprite);

        Warrior_StrikeComboBuff2 warrior_StrikeComboBuff2 = new Warrior_StrikeComboBuff2();
        ListOfAllBuffs.Add("Warrior_StrikeComboBuff2", warrior_StrikeComboBuff2);
        ListOfAllBuffSprites.Add("Warrior_StrikeComboBuff2", this.warrior_StrikeComboBuff2_Sprite);

        Warrior_SlashComboBuff2 warrior_SlashComboBuff2 = new Warrior_SlashComboBuff2();
        ListOfAllBuffs.Add("Warrior_SlashComboBuff2", warrior_SlashComboBuff2);
        ListOfAllBuffSprites.Add("Warrior_SlashComboBuff2", warrior_SlashComboBuff2_Sprite);

        Warrior_StingComboBuff2 warrior_StingComboBuff2 = new Warrior_StingComboBuff2();
        ListOfAllBuffs.Add("Warrior_StingComboBuff2", warrior_StingComboBuff2);
        ListOfAllBuffSprites.Add("Warrior_StingComboBuff2", warrior_StingComboBuff2_Sprite);

        Warrior_OffensiveStanceBuff warrior_OffensiveStanceBuff = new Warrior_OffensiveStanceBuff();
        ListOfAllBuffs.Add("Warrior_OffensiveStanceBuff", warrior_OffensiveStanceBuff);
        ListOfAllBuffSprites.Add("Warrior_OffensiveStanceBuff", warrior_OffensiveStanceBuff_Sprite);

        Warrior_DefensiveStanceBuff warrior_DefensiveStanceBuff = new Warrior_DefensiveStanceBuff();
        ListOfAllBuffs.Add("Warrior_DefensiveStanceBuff", warrior_DefensiveStanceBuff);
        ListOfAllBuffSprites.Add("Warrior_DefensiveStanceBuff", warrior_DefensiveStanceBuff_Sprite);

        Warrior_TearingSlashDoT warrior_TearingSlashDoT = new Warrior_TearingSlashDoT();
        ListOfAllBuffs.Add("Warrior_TearingSlashDoT", warrior_TearingSlashDoT);
        ListOfAllBuffSprites.Add("Warrior_TearingSlashDoT", warrior_TearingSlashDoT_Sprite);

        Warrior_DevastatingStrikeDebuff warrior_DevastatingStrikeDebuff = new Warrior_DevastatingStrikeDebuff();
        ListOfAllBuffs.Add("Warrior_DevastatingStrikeDebuff", warrior_DevastatingStrikeDebuff);
        ListOfAllBuffSprites.Add("Warrior_DevastatingStrikeDebuff", warrior_DevastatingStrikeDebuff_Sprite);
    }
}
