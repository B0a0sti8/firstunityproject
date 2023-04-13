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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        HoTBuff hoTBuff = new HoTBuff();
        SpeedBoostBuff speedBoostBuff = new SpeedBoostBuff();
        TheWayOfTheChickenDamageDebuff testWayOfTheChickenDamageDebuff = new TheWayOfTheChickenDamageDebuff();
        AttackSpeedBoostBuff attackSpeedBoostBuff = new AttackSpeedBoostBuff();

        ListOfAllBuffs.Add("HoTBuff", hoTBuff);
        ListOfAllBuffs.Add("speedBoostBuff", speedBoostBuff);
        ListOfAllBuffs.Add("TestWayOfTheChickenDamageDebuff", testWayOfTheChickenDamageDebuff);
        ListOfAllBuffs.Add("AttackSpeedBoostBuff", attackSpeedBoostBuff);
    }
}
