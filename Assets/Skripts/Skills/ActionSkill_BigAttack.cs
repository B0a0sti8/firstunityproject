using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSkill_BigAttack : MonoBehaviour
{
    // public Player player; // drag in Character to access Player skript (for health)

    // public PlayerController playerController; // drag in Character to access PlayerController skript (for speed)

    bool noCooldown = true;

    public CountDown countDown;

    public void BigAttack()
    {
        if (noCooldown)
        {
            noCooldown = false;
            //////////
            Debug.Log("Activate BigAttack: 400 Damage");
            //////////
            gameObject.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
            countDown.timeLeft = 30;
            StartCoroutine(WaitCooldown(30)); // Cooldown time
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
