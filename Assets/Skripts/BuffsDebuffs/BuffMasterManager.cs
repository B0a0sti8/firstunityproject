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
    [SerializeField] Sprite warrior_BrutalStrike_ComboBuff_Sprite;
    [SerializeField] Sprite warrior_SweepingSlash_ComboBuff_Sprite;

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
        Warrior_BrutalStrike_ComboBuff warrior_BrutalStrike_ComboBuff = new Warrior_BrutalStrike_ComboBuff();
        ListOfAllBuffs.Add("Warrior_BrutalStrike_ComboBuff", warrior_BrutalStrike_ComboBuff);
        ListOfAllBuffSprites.Add("Warrior_BrutalStrike_ComboBuff", warrior_BrutalStrike_ComboBuff_Sprite);

        Warrior_SweepingSlash_ComboBuff warrior_SweepingSlash_ComboBuff = new Warrior_SweepingSlash_ComboBuff();
        ListOfAllBuffs.Add("Warrior_SweepingSlash_ComboBuff", warrior_SweepingSlash_ComboBuff);
        ListOfAllBuffSprites.Add("Warrior_SweepingSlash_ComboBuff", warrior_SweepingSlash_ComboBuff_Sprite);
    }
}
