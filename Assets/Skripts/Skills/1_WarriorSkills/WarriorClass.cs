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

    public bool hasSweepingSlashComboBuff = false;
    private float sweepingSlashComboTimer = 0;

    public bool hasBrutalStrikeComboBuff = false;
    private float brutalStrikeComboTimer = 0;


    private void Awake()
    {
        //meleeSkillsRadiusIncrease = 5f;
        PLAYER = transform.parent.parent;
        playerStats = PLAYER.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (hasSweepingSlashComboBuff)
        {
            if (sweepingSlashComboTimer >= 5) { sweepingSlashComboTimer = 0; hasSweepingSlashComboBuff = false; }
            else sweepingSlashComboTimer += Time.deltaTime;
        }

        if (hasBrutalStrikeComboBuff)
        {
            if (brutalStrikeComboTimer >= 5) { brutalStrikeComboTimer = 0; hasBrutalStrikeComboBuff = false; }
            else brutalStrikeComboTimer += Time.deltaTime;
        }
    }
}
