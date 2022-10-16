using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarScript : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        //levelText = transform.parent.Find("PlayerLevel").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public void SetXPBar(int currentXP, int maxXP)
    {
        if (currentXP > maxXP)
        {
            xpText.text = string.Format("{0} / {1}", maxXP, maxXP);
            slider.value = maxXP;
        }
        else
        {
            xpText.text = string.Format("{0} / {1}", currentXP, maxXP);
            slider.value = currentXP;
        }
        
        slider.maxValue = maxXP;
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void UpdateLevel(int level)
    {
        if (levelText == null)
        {
            levelText = transform.parent.Find("PlayerLevel").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        }
        levelText.text = string.Format("{0}", level);
    }
}
