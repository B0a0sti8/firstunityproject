using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// AnimTime = GCEarlyTime = OwnCooldownEarlyTime
// GCTime >= GCEarlyTime
// ownCooldownTime >= ownCooldownEarlyTime

public class MasterChecks : MonoBehaviour
{
    // Parameter für die Animationen
    public bool masterAnimationActive = false;
    public float masterAnimTime;
    public float masterAnimTimeLeft;

    // Parameter für Global Cooldown und Einreihung, wenn ein Skill etwas zu früh gedrückt wird
    public bool masterGCActive = false;
    float masterGCTimeBase;
    public float masterGCTimeModified;
    public float masterGCTimeLeft;
    public float masterGCEarlyTime;
    public float masterOwnCooldownEarlyTime;

    // Parameter wenn momentan ein Skill gecastet wird.
    public bool masterIsSkillInQueue = false;
    public float castTimeMax;
    public float castTimeCurrent;
    public bool isSkillInterrupted = false;
    public bool masterIsCastFinished = false;

    public bool hasUnusedSpell = false;
    public GameObject myUnusedSpellIndicator;
    public SkillPrefab myUnusedSpell;

    GameObject PLAYER;
    PlayerStats playerStats;
    private Camera mainCam;

    private void Start()
    {
        mainCam = GameObject.Find("CameraMama").transform.Find("MainKamera").GetComponent<Camera>();
        PLAYER = transform.parent.parent.gameObject;
        playerStats = PLAYER.GetComponent<PlayerStats>();

        masterGCTimeBase = 0.5f;
        masterGCTimeModified = 0.5f;
        masterGCEarlyTime = 1f;
        masterOwnCooldownEarlyTime = 1f;
        masterAnimTime = 0.5f;
    }

    void LateUpdate()
    {
        float attackSpeedModifier = 1 - (playerStats.actionSpeed.GetValue() / 100);
        masterGCTimeModified = masterGCTimeBase * attackSpeedModifier;

        // Schaut ob ein AnimationsCooldown am laufen ist. Und lässt Zeit runterticken, wenn ja
        if (masterAnimTimeLeft > 0) masterAnimTimeLeft -= Time.deltaTime;
        else 
        {
            if (masterAnimationActive)
            {
                masterAnimationActive = false;
                masterAnimTimeLeft = 0f;
            }
        }

        // Schaut ob der Global Cooldown am laufen ist. Und lässt Zeit runterticken, wenn ja
        if (masterGCTimeLeft > 0) masterGCTimeLeft -= Time.deltaTime;
        else 
        {
            if (masterGCActive)
            {
                masterGCActive = false;
                masterGCTimeLeft = 0f;
            }
        }

        // Schaut ob der Skill unterbrochen wurde. Wenn nicht, wird die Zeit weiterlaufen gelassen. Wenn die Zeit um ist, wird SkillEffekt ausgelöst?
        if (playerStats.isCurrentlyCasting)
        {
            if (!isSkillInterrupted)
            {
                if (castTimeCurrent > 0) castTimeCurrent -= Time.deltaTime;
                else
                {
                    playerStats.isCurrentlyCasting = false;
                    castTimeCurrent = 0f;
                    castTimeMax = 0f;
                    masterIsCastFinished = true;
                    //Debug.Log("Gubl!" + masterIsCastFinished);
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

        if (myUnusedSpell != null || myUnusedSpellIndicator != null || hasUnusedSpell == true)
        {
            Vector3 mouseScreenposition = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            myUnusedSpellIndicator.transform.position = new Vector3(mouseScreenposition.x, mouseScreenposition.y, 0);

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Destroy(myUnusedSpellIndicator);
                myUnusedSpell = null;
                myUnusedSpellIndicator = null;
                hasUnusedSpell = false;
            }
            else if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                myUnusedSpell.circleAim = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                myUnusedSpell.GlobalCooldownCheck();


                Destroy(myUnusedSpellIndicator);
                myUnusedSpell = null;
                myUnusedSpellIndicator = null;
                hasUnusedSpell = false;
            }
        }
    }
}
