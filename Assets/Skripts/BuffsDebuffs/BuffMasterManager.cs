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

    // F�r allgemeine Buffs
    [SerializeField ]Sprite hoTBuffSprite;
    [SerializeField] Sprite speedBoostBuffSprite;
    [SerializeField] Sprite attackSpeedBoostBuffSprite;
    [SerializeField] Sprite stunnedEffectOnEnemiesSprite;

    // Zum testen
    [SerializeField] Sprite testWayOfTheChickenDamageDebuffSprite;

    // F�r Summoner-spezifische Buffs
    [SerializeField] Sprite summonInsectsOnEnemyDeathBuffSprite;


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

        // Spezifisch f�r Summoner-Classe
        SummonInsectsOnEnemyDeathBuff summonInsectsOnEnemyDeathBuff = new SummonInsectsOnEnemyDeathBuff();
        ListOfAllBuffs.Add("SummonInsectsOnEnemyDeathBuff", summonInsectsOnEnemyDeathBuff);
        ListOfAllBuffSprites.Add("SummonInsectsOnEnemyDeathBuff", summonInsectsOnEnemyDeathBuffSprite);
    }
}
