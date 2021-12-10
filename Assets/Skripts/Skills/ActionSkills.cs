using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class ActionSkills : MonoBehaviour
{
    public Player player; // drag in Character to access Player skript (for health)

    public PlayerController playerController; // drag in Character to access PlayerController skript (for speed)


    public bool skillAnimationOn = false;

    bool noCooldownSpeedboost = true;
    bool noCooldownGainLife = true;
    bool noCooldownGainLifeHot = true;
    bool noCooldownOverHeal = true;

    public CountDown countDownSpeedBoost;
    public CountDown countDownGainLife;
    public CountDown countDownGainLifeHot;
    public CountDown countDownOverHeal;

    public Button buttonSpeedBoost;
    public Button buttonGainLife;
    public Button buttonGainLifeHot;
    public Button buttonOverHeal;


    bool onGlobalCooldown = false;
    bool skillInGCQueue = false;
    GameObject[] weaponSkills;

    bool combo2ready = false;
    bool combo3ready = false;

    public GlobalCountDown globalCountDown;
    public float globalCoolDownTime = 2f;

    bool globalCooldownUpPitch = false;


    void Awake()
    {
        weaponSkills = GameObject.FindGameObjectsWithTag("WeaponSkillCD");
    }

    void Update()
    {
        if (skillAnimationOn)
        {
            StartCoroutine(WaitSkillAnimation(0.5f));
            IEnumerator WaitSkillAnimation(float time)
            {
                yield return new WaitForSeconds(time);
                skillAnimationOn = false;
            }
        }
    }

    public void SpeedBoost()
    {
        if (!skillAnimationOn)
        {
            if (noCooldownSpeedboost)
            {
                skillAnimationOn = true;
                noCooldownSpeedboost = false;
                //////////
                playerController._Speed += 5;
                StartCoroutine(Wait(10));
                IEnumerator Wait(float time)
                {
                    yield return new WaitForSeconds(time);
                    playerController._Speed -= 5;
                }
                //////////
                buttonSpeedBoost.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
                countDownSpeedBoost.timeLeft = 2; // start Button-text timer (not linked to the Cooldown timer!)
                StartCoroutine(WaitCooldown(2)); // Cooldown time
                IEnumerator WaitCooldown(float time)
                {
                    yield return new WaitForSeconds(time);
                    noCooldownSpeedboost = true;
                    buttonSpeedBoost.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
                skillInGCQueue = false;
                FindObjectOfType<AudioManager>().Play("HoverClick");
            }
            else
            {
                Debug.Log("Skill is currently on cooldown!");
                FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
            }
        } 
        else
        {
            if (!skillInGCQueue)
            {
                StartCoroutine(WaitForAnimation_SpeedBoost());
            } else
            {
                Debug.Log("There is already a skill in the NORMAL queue");
            }
        }
    }

    private IEnumerator WaitForAnimation_SpeedBoost()
    {
        Debug.Log("Too early. Wait For Animation. SpeedBoost in queue");
        skillInGCQueue = true;
        FindObjectOfType<AudioManager>().Play("HoverClickUpPitch");
        //globalCooldownUpPitch = true;
        yield return new WaitUntil(() => skillAnimationOn == false);
        SpeedBoost();
    }

    public void GainLife()
    {
        if (!skillAnimationOn)
        {
            if (noCooldownGainLife)
            {
                skillAnimationOn = true;
                noCooldownGainLife = false;
                //////////
                //player.currentHealth += 20;
                //////////
                buttonGainLife.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
                countDownGainLife.timeLeft = 10;
                StartCoroutine(WaitCooldown(10)); // Cooldown time
                IEnumerator WaitCooldown(float time)
                {
                    yield return new WaitForSeconds(time);
                    noCooldownGainLife = true;
                    buttonGainLife.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
                skillInGCQueue = false;
                FindObjectOfType<AudioManager>().Play("HoverClick");
            }
            else
            {
                Debug.Log("Skill is currently on cooldown!");
                FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
            }
        }
        else
        {
            if (!skillInGCQueue)
            {
                StartCoroutine(WaitForAnimation_GainLife());
            }
            else
            {
                Debug.Log("There is already a skill in the NORMAL queue");
            }
        }
    }

    private IEnumerator WaitForAnimation_GainLife()
    {
        Debug.Log("Too early. Wait For Animation. SpeedBoost in queue");
        skillInGCQueue = true;
        FindObjectOfType<AudioManager>().Play("HoverClickUpPitch");
        //globalCooldownUpPitch = true;
        yield return new WaitUntil(() => skillAnimationOn == false);
        GainLife();
    }

    public void GainLifeHot()
    {
        if (!skillAnimationOn)
        {
            if (noCooldownGainLifeHot)
            {
                skillAnimationOn = true;
                noCooldownGainLifeHot = false;
                //////////
                StartCoroutine(Hot(2f, 5));
                //////////
                buttonGainLifeHot.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
                countDownGainLifeHot.timeLeft = 15;
                StartCoroutine(WaitCooldown(15)); // Cooldown time
                IEnumerator WaitCooldown(float time)
                {
                    yield return new WaitForSeconds(time);
                    noCooldownGainLifeHot = true;
                    buttonGainLifeHot.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
                skillInGCQueue = false;
                FindObjectOfType<AudioManager>().Play("HoverClick");
            }
            else
            {
                Debug.Log("Skill is currently on cooldown");
                FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
            }
        }
        else
        {
            if (!skillInGCQueue)
            {
                StartCoroutine(WaitForAnimation_GainLifeHot());
            }
            else
            {
                Debug.Log("There is already a skill in the NORMAL queue");
            }
        }
    }

    private IEnumerator WaitForAnimation_GainLifeHot()
    {
        Debug.Log("Too early. Wait For Animation. SpeedBoost in queue");
        skillInGCQueue = true;
        FindObjectOfType<AudioManager>().Play("HoverClickUpPitch");
        //globalCooldownUpPitch = true;
        yield return new WaitUntil(() => skillAnimationOn == false);
        GainLifeHot();
    }

    public void OverHeal()
    {
        if (!skillAnimationOn)
        {
            if (noCooldownOverHeal)
            {
                skillAnimationOn = true;
                noCooldownOverHeal = false;
                //////////
                //player.maxHealth += 50;
                //player.currentHealth += 50;
                StartCoroutine(Wait(30));
                IEnumerator Wait(float time)
                {
                    yield return new WaitForSeconds(time);
                    //player.maxHealth -= 50;
                }
                //////////
                buttonOverHeal.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
                countDownOverHeal.timeLeft = 60;
                StartCoroutine(WaitCooldown(60)); // Cooldown time
                IEnumerator WaitCooldown(float time)
                {
                    yield return new WaitForSeconds(time);
                    noCooldownOverHeal = true;
                    buttonOverHeal.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
                skillInGCQueue = false;
                FindObjectOfType<AudioManager>().Play("HoverClick");
            }
            else
            {
                Debug.Log("Skill is currently on cooldown");
                FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
            }
        }
        else
        {
            if (!skillInGCQueue)
            {
                StartCoroutine(WaitForAnimation_OverHeal());
            }
            else
            {
                Debug.Log("There is already a skill in the NORMAL queue");
            }
        }
    }

    private IEnumerator WaitForAnimation_OverHeal()
    {
        Debug.Log("Too early. Wait For Animation. SpeedBoost in queue");
        skillInGCQueue = true;
        FindObjectOfType<AudioManager>().Play("HoverClickUpPitch");
        //globalCooldownUpPitch = true;
        yield return new WaitUntil(() => skillAnimationOn == false);
        OverHeal();
    }

    // WEAPON SKILLS //

    public void Attack1()
    {
        if (!skillAnimationOn)
        {
            if (onGlobalCooldown)
            {
                if (globalCountDown.timeLeft <= 0.5 && !skillInGCQueue)
                {
                    StartCoroutine(GlobalCooldownEarlyAttack1());
                }
                else if (globalCountDown.timeLeft <= 0.5 && skillInGCQueue)
                {
                    Debug.Log("There is already a skill in the queue!");
                }
                else
                {
                    Debug.Log("This skill is on Global Cooldown!");
                    FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
                }
            }
            else
            {
                /////
                Debug.Log("Activate Attack1: 120 Damage");
                combo2ready = true;
                combo3ready = false;
                /////
                GlobalCooldown();
                //FindObjectOfType<AudioManager>().Play("HoverClick");
            }
        }
    }

    private IEnumerator GlobalCooldownEarlyAttack1()
    {
        //Debug.Log(globalCountDown.timeLeft + " sec too early. Attack1 in queue");
        skillInGCQueue = true;
        FindObjectOfType<AudioManager>().Play("HoverClickUpPitch");
        globalCooldownUpPitch = true;
        yield return new WaitUntil(() => onGlobalCooldown == false);
        Attack1();
    }

    public void Attack2()
    {
        if (!skillAnimationOn)
        {
            if (onGlobalCooldown)
            {
                if (globalCountDown.timeLeft <= 0.5 && !skillInGCQueue)
                {
                    StartCoroutine(GlobalCooldownEarlyAttack2());
                }
                else if (globalCountDown.timeLeft <= 0.5 && skillInGCQueue)
                {
                    Debug.Log("There is already a skill in the queue!");
                }
                else
                {
                    Debug.Log("This skill is on Global Cooldown!");
                    FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
                }
            }
            else
            {
                /////
                if (combo2ready)
                {
                    Debug.Log("Activate Attack2: 150 Damage");
                    combo3ready = true;
                }
                else
                {
                    Debug.Log("Activate Attack2: 100 Damage");
                    combo3ready = false;
                }
                combo2ready = false;
                /////
                GlobalCooldown();
                //FindObjectOfType<AudioManager>().Play("HoverClick");
            }
        }
    }

    private IEnumerator GlobalCooldownEarlyAttack2()
    {
        //Debug.Log(globalCountDown.timeLeft + " sec too early. Attack2 in queue");
        skillInGCQueue = true;
        FindObjectOfType<AudioManager>().Play("HoverClickUpPitch");
        globalCooldownUpPitch = true;
        yield return new WaitUntil(() => onGlobalCooldown == false);
        Attack2();
    }

    public void Attack3()
    {
        if (!skillAnimationOn)
        {
            if (onGlobalCooldown)
            {
                if (globalCountDown.timeLeft <= 0.5 && !skillInGCQueue)
                {
                    StartCoroutine(GlobalCooldownEarlyAttack3());
                    // invoke
                    //Invoke("Attack3", ??);
                }
                else if (globalCountDown.timeLeft <= 0.5 && skillInGCQueue)
                {
                    Debug.Log("There is already a skill in the queue!");
                }
                else
                {
                    Debug.Log("This skill is on Global Cooldown!");
                    FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
                }
            }
            else
            {
                /////
                if (combo3ready)
                {
                    Debug.Log("Activate Attack3: 200 Damage");
                }
                else
                {
                    Debug.Log("Activate Attack3: 100 Damage");
                }
                combo2ready = false;
                combo3ready = false;
                /////
                GlobalCooldown();
            }
        }
    }

    private IEnumerator GlobalCooldownEarlyAttack3()
    {
        //Debug.Log(globalCountDown.timeLeft + " sec too early. Attack3 in queue");
        skillInGCQueue = true;
        FindObjectOfType<AudioManager>().Play("HoverClickUpPitch");
        globalCooldownUpPitch = true;
        yield return new WaitUntil(() => onGlobalCooldown == false);
        Attack3();
    }

    void GlobalCooldown()
    {
        skillAnimationOn = true;
        onGlobalCooldown = true;
        skillInGCQueue = false;
        globalCountDown.timeLeft = globalCoolDownTime;
        foreach (GameObject gObj in weaponSkills)
        {
            gObj.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
        }
        StartCoroutine(WaitGlobalCooldown(globalCoolDownTime)); // Cooldown time
        IEnumerator WaitGlobalCooldown(float time)
        {
            yield return new WaitForSeconds(time);
            onGlobalCooldown = false;
            foreach (GameObject gObj in weaponSkills)
            {
                gObj.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
        if (!globalCooldownUpPitch)
        {
            FindObjectOfType<AudioManager>().Play("HoverClick");
        }
        globalCooldownUpPitch = false;
    }

    IEnumerator Hot(float time, int healing)
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(time);
            //player.currentHealth += healing;
        }
    }

    //public void _____()
    //{
    //    if (noCooldown_____)
    //    {
    //        noCooldown_____ = false;
    //        //////////
    //        // ActionSkill code
    //        //////////
    //        button_____.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
    //        countDown_____.timeLeft = ___;
    //        StartCoroutine(WaitCooldown(___)); // Cooldown time
    //        IEnumerator WaitCooldown(float time)
    //        {
    //            yield return new WaitForSeconds(time);
    //            noCooldown_____ = true;
    //            button_____.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    //        }
    //        FindObjectOfType<AudioManager>().Play("HoverClick");
    //    }
    //    else
    //    {
    //        Debug.Log("Skill is currently on cooldown");
    //        FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
    //    }
    //}

    // 1. Create Button
    // 2. Drag in Source Image
    // 3. Create Input Action
    // 4. Create bool noCooldown_____ = true;
    //    Create public CountDown countDown_____;
    //    Create public Button button_____;
    // 5. Create public void _____ in ActionSkills skript
    // 6. Create in InputSkills skript:
    //    private void On_____()
    //    {
    //        actionSkills._____();
    //    }
    // 7. Create On Click () event on Button:
    //    Drag in Canvas Action Skills
    //    Choose ActionSkills -> _____ ()
    // 8. Add Count Down (Skript) Component to Button
    //    Drag Text (child of Button) in Text
    // 9. ActionSkills Skript on Canvas Action Skills:
    //    Count Down _____: Drag in Button
    //    Button _____: Drag in Button





    // gObj.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "";
    // buttonSpeedBoost.transform.GetChild(0).gameObject.SetActive(true); // enable/disable Button-text
    // buttonSpeedBoost.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "20"; // change Button-Text

    //StartCoroutine(WaitGlobalCooldown(globalCountDown.timeLeft + 0.001f));
    //IEnumerator WaitGlobalCooldown(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    //Debug.Log("Performing now Attack3");
    //    Attack3();
    //}
}
