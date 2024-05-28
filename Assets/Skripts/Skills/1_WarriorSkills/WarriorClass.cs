using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WarriorClass : NetworkBehaviour
{
    Transform PLAYER;
    PlayerStats playerStats;

    public float meleeSkillsRadiusIncrease = 0;

    public bool hasSlashCombo1Buff = false;
    private float slashCombo1Timer = 0;

    public bool hasStrikeCombo1Buff = false;
    private float StrikeCombo1Timer = 0;

    public bool hasStingCombo1Buff = false;
    private float StingCombo1Timer = 0;

    public bool hasSlashCombo2Buff = false;
    private float slashCombo2Timer = 0;

    public bool hasStrikeCombo2Buff = false;
    private float StrikeCombo2Timer = 0;

    public bool hasStingCombo2Buff = false;
    private float StingCombo2Timer = 0;

    public bool defensiveStanceOn = false;
    public bool offensiveStanceOn = false;

    public bool hasTearingSlashBleed;


    private void Awake()
    {
        //meleeSkillsRadiusIncrease = 5f;
        PLAYER = transform.parent.parent;
        playerStats = PLAYER.GetComponent<PlayerStats>();

        hasTearingSlashBleed = true;
    }

    private void Update()
    {
        if (hasSlashCombo1Buff)
        {
            if (slashCombo1Timer >= 5) { slashCombo1Timer = 0; hasSlashCombo1Buff = false; }
            else slashCombo1Timer += Time.deltaTime;
        }

        if (hasStrikeCombo1Buff)
        {
            if (StrikeCombo1Timer >= 5) { StrikeCombo1Timer = 0; hasStrikeCombo1Buff = false; }
            else StrikeCombo1Timer += Time.deltaTime;
        }

        if (hasStingCombo1Buff)
        {
            if (StingCombo1Timer >= 5) { StingCombo1Timer = 0; hasStingCombo1Buff = false; }
            else StingCombo1Timer += Time.deltaTime;
        }

        if (hasSlashCombo2Buff)
        {
            if (slashCombo2Timer >= 5) { slashCombo2Timer = 0; hasSlashCombo2Buff = false; }
            else slashCombo2Timer += Time.deltaTime;
        }

        if (hasStrikeCombo2Buff)
        {
            if (StrikeCombo2Timer >= 5) { StrikeCombo2Timer = 0; hasStrikeCombo2Buff = false; }
            else StrikeCombo2Timer += Time.deltaTime;
        }

        if (hasStingCombo2Buff)
        {
            if (StingCombo2Timer >= 5) { StingCombo2Timer = 0; hasStingCombo2Buff = false; }
            else StingCombo2Timer += Time.deltaTime;
        }
    }

    [ServerRpc]
    public void ToggleDefensiveStanceServerRpc(bool switchOn)
    {
        ToggleDefensiveStanceClientRpc(switchOn);
    }

    [ClientRpc]
    public void ToggleDefensiveStanceClientRpc(bool switchOn)
    {
        defensiveStanceOn = switchOn;            
    }

    [ServerRpc]
    public void ToggleOffensiveStanceServerRpc(bool switchOn)
    {
        ToggleOffensiveStanceClientRpc(switchOn);
    }

    [ClientRpc]
    public void ToggleOffensiveStanceClientRpc(bool switchOn)
    {
        offensiveStanceOn = switchOn;
    }
}
