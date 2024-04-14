using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    [SerializeField] private TextMeshProUGUI myHealthText;

    private void Awake()
    {
        myHealthText = transform.Find("Health Text").GetComponent<TextMeshProUGUI>();
        slider = GetComponent<Slider>();
        fill = transform.Find("Health Fill").GetComponent<Image>();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        string myCurrentText1 = myHealthText.text;
        string[] myCurrentText = myCurrentText1.Split(" / ");

        myCurrentText[1] = health.ToString();
        myHealthText.text = myCurrentText[0] + " / " + myCurrentText[1];
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);

        string myCurrentText1 = myHealthText.text;
        string[] myCurrentText = myCurrentText1.Split(" / ");
        
        myCurrentText[0] = health.ToString();
        myHealthText.text = myCurrentText[0] + " / " + myCurrentText[1];
    }
}
