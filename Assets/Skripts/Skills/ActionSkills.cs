using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ActionSkills : MonoBehaviour
{
    public Player player; // drag in Character to access Player skript (for health)

    public PlayerController playerController; // drag in Character to access PlayerController skript (for speed)

    bool onGlobalCooldown = false;
    GameObject[] weaponSkills;

    bool combo2ready = false;
    bool combo3ready = false;

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

    void Awake()
    {
        weaponSkills = GameObject.FindGameObjectsWithTag("WeaponSkillCD");
    }

    public void SpeedBoost()
    {
        if (noCooldownSpeedboost)
        {
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
            countDownSpeedBoost.timeLeft = 20; // start Button-text timer (not linked to the Cooldown timer!)
            StartCoroutine(WaitCooldown(20)); // Cooldown time
            IEnumerator WaitCooldown(float time)
            {
                yield return new WaitForSeconds(time);
                noCooldownSpeedboost = true;
                buttonSpeedBoost.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
            FindObjectOfType<AudioManager>().Play("HoverClick");
        } else
        {
            Debug.Log("Skill is currently on cooldown!");
            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
        }
    }

    public void GainLife()
    {
        if (noCooldownGainLife)
        {
            noCooldownGainLife = false;
            //////////
            player.currentHealth += 20;
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
            FindObjectOfType<AudioManager>().Play("HoverClick");
        } else
        {
            Debug.Log("Skill is currently on cooldown!");
            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
        }
    }

    public void GainLifeHot()
    {
        if (noCooldownGainLifeHot)
        {
            noCooldownGainLifeHot = false;
            //////////
            StartCoroutine(Wait(2));
            StartCoroutine(Wait(4));
            StartCoroutine(Wait(6));
            StartCoroutine(Wait(8));
            StartCoroutine(Wait(10));
            IEnumerator Wait(float time)
            {
                yield return new WaitForSeconds(time);
                player.currentHealth += 5;
            }
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
            FindObjectOfType<AudioManager>().Play("HoverClick");
        } else
        {
            Debug.Log("Skill is currently on cooldown");
            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
        }
    }

    public void OverHeal()
    {
        if (noCooldownOverHeal)
        {
            noCooldownOverHeal = false;
            //////////
            player.maxHealth += 50;
            player.currentHealth += 50;
            StartCoroutine(Wait(30));
            IEnumerator Wait(float time)
            {
                yield return new WaitForSeconds(time);
                player.maxHealth -= 50;
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
            FindObjectOfType<AudioManager>().Play("HoverClick");
        }
        else
        {
            Debug.Log("Skill is currently on cooldown");
            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
        }
    }

    // WEAPON SKILLS //

    public void Attack1()
    {
        if (onGlobalCooldown)
        {
            Debug.Log("This skill is on Global Cooldown!");
            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
        } else
        {
            Debug.Log("Activate Attack1: 120 Damage");
            combo2ready = true;
            combo3ready = false;
            GlobalCooldown();
            FindObjectOfType<AudioManager>().Play("HoverClick");
        }
    }

    public void Attack2()
    {
        if (onGlobalCooldown)
        {
            Debug.Log("This skill is on Global Cooldown!");
            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
        }
        else
        {
            if (combo2ready)
            {
                Debug.Log("Activate Attack2: 150 Damage");
                combo3ready = true;
            } else
            {
                Debug.Log("Activate Attack2: 100 Damage");
                combo3ready = false;
            }
            combo2ready = false;
            GlobalCooldown();
            FindObjectOfType<AudioManager>().Play("HoverClick");
        }
    }

    public void Attack3()
    {
        if (onGlobalCooldown)
        {
            Debug.Log("This skill is on Global Cooldown!");
            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
        }
        else
        {
            if (combo3ready)
            {
                Debug.Log("Activate Attack3: 200 Damage");
            } else
            {
                Debug.Log("Activate Attack3: 100 Damage");
            }
            combo2ready = false;
            combo3ready = false;
            GlobalCooldown();
            FindObjectOfType<AudioManager>().Play("HoverClick");
        }
    }

    void GlobalCooldown()
    {
        onGlobalCooldown = true;
        foreach (GameObject gObj in weaponSkills)
        {
            gObj.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
            gObj.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "2";
        }

        StartCoroutine(WaitGlobalCooldown(2)); // Cooldown time
        IEnumerator WaitGlobalCooldown(float time)
        {
            yield return new WaitForSeconds(time);
            onGlobalCooldown = false;
            foreach (GameObject gObj in weaponSkills)
            {
                gObj.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                gObj.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "";
            }
        }
    }

    //buttonSpeedBoost.transform.GetChild(0).gameObject.SetActive(true); // enable/disable Button-text
    //buttonSpeedBoost.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "20"; // change Button-Text

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
}
