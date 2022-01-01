using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Hot : MasterSchmuff
{
    public Buff thisBuff;

    [Header("Hot-Stats")]
    public float hotTickTime = 2f;
    public int hotHealing = 5;
    //public float buffDuration = 10f;

    int numberTicks;
    bool stopBuff = false;

    public override void BuffEffect(PlayerStats playerStats, float duration)
    {
        stopBuff = false;

        numberTicks = (int)(duration / hotTickTime);

        StartHot(playerStats, hotTickTime, hotHealing, numberTicks);
        //Debug.Log(gameObject);
        //Debug.Log(gameObject.activeInHierarchy);
        //StartCoroutine(HealingOverTime(playerStats, hotTickTime, hotHealing, buffDuration));
    }

    public override void RemoveBuff(PlayerStats playerStats)
    {
        stopBuff = true;
        BuffManager.instance.Remove(thisBuff);
    }

    public async void StartHot(PlayerStats playerStats, float tickTime, int healing, float numberTicks)
    {
        Debug.Log("In Start Hot / Number of ticks: " + numberTicks);
        for (int i = 0; i < numberTicks; i++)
        {
            Debug.Log("i = " + i);
            //yield return new WaitForSeconds(tickTime);
            if (stopBuff)
            {
                Debug.Log("StopBuff early!!");
                i = (int) numberTicks + 1;
            }
            else
            {
                Debug.Log("await TickEverySeconds");
                await TickEverySeconds(playerStats, tickTime, healing);
                //playerStats.currentHealth += healing;
            }
        }
        Debug.Log("For Loop over");
    }

    public async Task TickEverySeconds(PlayerStats playerStats, float tickTime, int healing)
    {
        bool doOnce = true;
        var end = Time.time + tickTime;
        while (Time.time < end)
        {
            if (doOnce)
            {
                doOnce = false;
                Debug.Log("Gain Healinge");
                playerStats.currentHealth += healing;
            }
            await Task.Yield();
        }
    }

    //IEnumerator HealingOverTime(PlayerStats playerStats, float tickTime, int healing, float duration)
    //{
    //    for (int i = 0; i < duration / tickTime; i++)
    //    {
    //        yield return new WaitForSeconds(tickTime);
    //        if (stopBuff)
    //        {
    //            i = (int) duration + 1;
    //        }
    //        else
    //        {
    //            playerStats.currentHealth += healing;
    //        }
    //    }
    //}
}
