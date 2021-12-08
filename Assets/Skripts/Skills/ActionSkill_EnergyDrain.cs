using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSkill_EnergyDrain : MonoBehaviour
{
    public Player player; // drag in Character to access Player skript (for health)

    // public PlayerController playerController; // drag in Character to access PlayerController skript (for speed)

    public ActionSkills actionSkills;

    bool noCooldown = true;
    
    public CountDown countDown;

    public void EnergyDrain()
    {
        if (!actionSkills.skillAnimationOn)
        {
            if (noCooldown)
            {
                actionSkills.skillAnimationOn = true;
                noCooldown = false;
                //////////
                Debug.Log("Activate EnergyDrain: 300 Damage");
                player.currentHealth += 30;
                //////////
                gameObject.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
                countDown.timeLeft = 60;
                StartCoroutine(WaitCooldown(60)); // Cooldown time
                IEnumerator WaitCooldown(float time)
                {
                    yield return new WaitForSeconds(time);
                    noCooldown = true;
                    gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
                FindObjectOfType<AudioManager>().Play("HoverClick");
            }
            else
            {
                Debug.Log("Skill is currently on cooldown");
                FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
            }
        }
    }
}
