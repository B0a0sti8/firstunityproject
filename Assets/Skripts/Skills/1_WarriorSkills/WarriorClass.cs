using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WarriorClass : NetworkBehaviour
{
    Transform PLAYER;
    PlayerStats playerStats;
    public float meleeSkillsRadiusIncrease;


    private void Awake()
    {
        meleeSkillsRadiusIncrease = 5f;
        PLAYER = transform.parent.parent;
        playerStats = PLAYER.GetComponent<PlayerStats>();
    }
}
