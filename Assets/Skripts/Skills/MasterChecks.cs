using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// AnimTime = GCEarlyTime = OwnCooldownEarlyTime
// GCTime >= GCEarlyTime
// ownCooldownTime >= ownCooldownEarlyTime

public class MasterChecks : MonoBehaviour
{
    public bool masterAnimationActive = false;
    public float masterAnimTime = 0.5f;
    public float masterAnimTimeLeft;

    public bool masterGCActive = false;
    float masterGCTimeBase = 1.5f;
    public float masterGCTimeModified = 1.5f;
    public float masterGCTimeLeft;
    public float masterGCEarlyTime = 0.5f;

    //public float masterOwnCooldownModifier = 1f;
    public float masterOwnCooldownEarlyTime = 0.5f;

    public bool masterIsSkillInQueue = false;
    public float castTimeMax;
    public float castTimeCurrent;
    public bool isSkillInterrupted = false;
    public bool masterIsCastFinished = false;
    GameObject PLAYER;

    PlayerStats playerStats;
    private void Start()
    {
        PLAYER = transform.parent.transform.parent.gameObject;
        playerStats = PLAYER.GetComponent<PlayerStats>();
    }

    void Update()
    {
        float attackSpeedModifier = 1 - (playerStats.attackSpeed.GetValue() / 100);
        masterGCTimeModified = masterGCTimeBase * attackSpeedModifier;

        if (masterAnimTimeLeft > 0)
        {
            masterAnimTimeLeft -= Time.deltaTime;
        }
        else // time <= 0
        {
            if (masterAnimationActive)
            {
                masterAnimationActive = false;
                masterAnimTimeLeft = 0f;
            }
        }

        if (masterGCTimeLeft > 0)
        {
            masterGCTimeLeft -= Time.deltaTime;
        }
        else // time <= 0
        {
            if (masterGCActive)
            {
                masterGCActive = false;
                masterGCTimeLeft = 0f;
            }
        }

        if (playerStats.isCurrentlyCasting)
        {
            if (!isSkillInterrupted)
            {
                if (castTimeCurrent > 0)
                {
                    castTimeCurrent -= Time.deltaTime;
                }
                else
                {
                    playerStats.isCurrentlyCasting = false;
                    castTimeCurrent = 0f;
                    castTimeMax = 0f;
                    masterIsCastFinished = true;
                    Debug.Log("Gubl!" + masterIsCastFinished);
                    PLAYER.transform.Find("PlayerParticleSystems").Find("CastingParticles").gameObject.GetComponent<ParticleSystem>().Stop();
                }
            }
            else
            {
                Debug.Log("Skill cast unterbrochen");
                playerStats.isCurrentlyCasting = false;
                castTimeCurrent = 0;
                castTimeMax = 0f;
                PLAYER.transform.Find("PlayerParticleSystems").Find("CastingParticles").gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }
    }
}
