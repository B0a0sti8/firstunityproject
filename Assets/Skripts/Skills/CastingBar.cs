using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastingBar : MonoBehaviour
{
    GameObject castingBarUI;
    GameObject PLAYER;
    PlayerStats playerStats;
    MasterChecks masterChecks;
    // Start is called before the first frame update
    void Start()
    {
        castingBarUI = transform.Find("Casting Bar").gameObject;
        castingBarUI.SetActive(false);
        PLAYER = transform.parent.parent.gameObject;
        playerStats = PLAYER.GetComponent<PlayerStats>();
        masterChecks = PLAYER.transform.Find("Own Canvases").Find("Canvas Action Skills").GetComponent<MasterChecks>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.isCurrentlyCasting)
        {
            castingBarUI.SetActive(true);
            if (playerStats.castingBarChanneling)
            {
                castingBarUI.transform.Find("Background").Find("Background").Find("Fill Image").GetComponent<Image>().fillAmount = masterChecks.castTimeCurrent / masterChecks.castTimeMax;
            }
            else
            {
                castingBarUI.transform.Find("Background").Find("Background").Find("Fill Image").GetComponent<Image>().fillAmount = 1 - masterChecks.castTimeCurrent / masterChecks.castTimeMax;
            }
            castingBarUI.transform.Find("Background").Find("Background").Find("CastDuration").GetComponent<TextMeshProUGUI>().text = masterChecks.castTimeCurrent.ToString("F1");
            castingBarUI.transform.Find("Background").Find("Background").Find("SpellName").GetComponent<TextMeshProUGUI>().text = playerStats.castingBarText;
            castingBarUI.transform.Find("Background").Find("Background").Find("Image").GetComponent<Image>().sprite = playerStats.castingBarImage;

        }
        else
        {
            castingBarUI.SetActive(false);
        }
    }
}
