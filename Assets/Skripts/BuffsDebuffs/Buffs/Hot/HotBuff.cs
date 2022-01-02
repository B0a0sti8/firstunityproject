using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class HotBuff : MasterSchmuff
{
    public Buff thisBuff;

    [Header("Hot-Stats")]
    public float hotTickTime = 2f;
    public int hotHealing = 5;

    int numberTicks;

    CancellationTokenSource tokenSource = null;

    public override void BuffEffect(PlayerStats playerStats, float duration)
    {
        numberTicks = (int)(duration / hotTickTime);
        StartStartHot(playerStats, hotTickTime, hotHealing, numberTicks);
    }

    public async void StartStartHot(PlayerStats playerStats, float tickTime, int healing, float numberTicks)
    {
        try{ await StartHot(playerStats, hotTickTime, hotHealing, numberTicks); }
        catch (System.Exception) { }
        finally { tokenSource.Dispose(); Debug.Log("Disposed"); }
    }

    public override void RemoveBuff(PlayerStats playerStats)
    {
        Debug.Log("Cancel 1");
        tokenSource.Cancel();
        Debug.Log("Cancel 2");
        BuffManager.instance.Remove(thisBuff);
        Debug.Log(gameObject);
    }

    public async Task StartHot(PlayerStats playerStats, float tickTime, int healing, float numberTicks)
    {
        tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        Debug.Log("In Start Hot / Number of ticks: " + numberTicks);
        for (int i = 0; i < numberTicks; i++)
        {
            Debug.Log("i = " + i);
            if (token.IsCancellationRequested)
            {
                Debug.Log("Token Cancjel lool");
                return;
            }
            await TickEverySeconds(playerStats, tickTime, healing);
        }
        Debug.Log("For Loop over");
    }

    public async Task TickEverySeconds(PlayerStats playerStats, float tickTime, int healing)
    {
        Debug.Log("Kriegt Leben");
        playerStats.currentHealth += healing;
        var end = Time.time + tickTime;
        while (Time.time < end)
        {
            await Task.Yield();
        }
    }
}
