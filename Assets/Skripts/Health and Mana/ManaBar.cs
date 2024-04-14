using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    [SerializeField] private TextMeshProUGUI myManaText;

    private void Awake()
    {
        myManaText = transform.Find("Mana Text").GetComponent<TextMeshProUGUI>();
        slider = GetComponent<Slider>();
        fill = transform.Find("Mana Fill").GetComponent<Image>();
    }

    public void SetMaxMana(int mana)
    {
        slider.maxValue = mana;
        //slider.value = mana;

        fill.color = gradient.Evaluate(1f);

        string myCurrentText1 = myManaText.text;
        string[] myCurrentText = myCurrentText1.Split(" / ");

        myCurrentText[1] = mana.ToString();
        myManaText.text = myCurrentText[0] + " / " + myCurrentText[1];
    }

    public void SetMana(int mana)
    {
        slider.value = mana;

        fill.color = gradient.Evaluate(slider.normalizedValue);

        string myCurrentText1 = myManaText.text;
        string[] myCurrentText = myCurrentText1.Split(" / ");

        myCurrentText[0] = mana.ToString();
        myManaText.text = myCurrentText[0] + " / " + myCurrentText[1];
    }
}
