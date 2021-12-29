using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// inspired by: Code Monkey - Dynamic Tooltip in Unity! (Resizable, Follows Mouse, Edge Detection)

public class TooltipScreenSpaceUIAdvanced : MonoBehaviour
{
    public static TooltipScreenSpaceUIAdvanced Instance { get; private set; }


    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform playerRectTransform;

    RectTransform backgroundRectTransform;
    RectTransform rectTransform;

    TextMeshProUGUI textMeshProName;
    TextMeshProUGUI textMeshProDescription;
    Image image;
    TextMeshProUGUI textMeshProType;
    TextMeshProUGUI textMeshProCooldown;
    TextMeshProUGUI textMeshProCosts;

    //private System.Func<string> getTooltipTextFunc;


    private void Awake()
    {
        Instance = this;

        backgroundRectTransform = transform.Find("Tooltip Background").GetComponent<RectTransform>();
        rectTransform = transform.GetComponent<RectTransform>();

        textMeshProName = transform.Find("Tooltip Text Skillname").GetComponent<TextMeshProUGUI>();
        textMeshProDescription = transform.Find("Tooltip Description").GetComponent<TextMeshProUGUI>();
        image = transform.Find("Tooltip Image").GetComponent<Image>();
        textMeshProType = transform.Find("Tooltip Text Skilltype").GetComponent<TextMeshProUGUI>();
        textMeshProCooldown = transform.Find("Tooltip Text Skillcooldown").GetComponent<TextMeshProUGUI>();
        textMeshProCosts = transform.Find("Tooltip Text Skillcosts").GetComponent<TextMeshProUGUI>();

        HideTooltip();
    }

    void SetText(string tooltipName, string tooltipDescription, Sprite tooltipSprite, string tooltipType, string tooltipCooldown, string tooltipCosts)
    {
        textMeshProName.SetText(tooltipName);
        textMeshProDescription.SetText(tooltipDescription);
        image.sprite = tooltipSprite;
        textMeshProType.SetText(tooltipType);
        textMeshProCooldown.SetText(tooltipCooldown);
        textMeshProCosts.SetText(tooltipCosts);

        textMeshProName.ForceMeshUpdate();
        textMeshProDescription.ForceMeshUpdate();
        textMeshProType.ForceMeshUpdate();
        textMeshProCooldown.ForceMeshUpdate();
        textMeshProCosts.ForceMeshUpdate();


        //Vector2 textSize = textMeshPro.GetRenderedValues(false);
        //Vector2 paddingSize = new Vector2(8, 8);
        //backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }

    private void Update()
    {
        //SetText(getTooltipTextFunc());

        float scaleAdjust = canvasRectTransform.localScale.x * playerRectTransform.localScale.x;
        Vector2 anchoredPosition = Mouse.current.position.ReadValue() / scaleAdjust;

        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            // Tooltip left screen on right side
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }

        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            // Tooltip left screen on top side
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void ShowTooltip(string tooltipName, string tooltipDescription, Sprite tooltipSprite, string tooltipType, string tooltipCooldown, string tooltipCosts)
    {
        gameObject.SetActive(true);
        SetText(tooltipName, tooltipDescription, tooltipSprite, tooltipType, tooltipCooldown, tooltipCosts);
    }

    //private void ShowTooltip(string tooltipText)
    //{
    //    ShowTooltip(() => tooltipText);
    //}
    //private void ShowTooltip(System.Func<string> getTooltipTextFunc)
    //{
    //    this.getTooltipTextFunc = getTooltipTextFunc;
    //    gameObject.SetActive(true);
    //    SetText(getTooltipTextFunc());
    //}

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string tooltipName, string tooltipDescription, Sprite tooltipSprite, string tooltipType, string tooltipCooldown, string tooltipCosts)
    {
        Instance.ShowTooltip(tooltipName, tooltipDescription, tooltipSprite, tooltipType, tooltipCooldown, tooltipCosts);
    }

    //public static void ShowTooltip_Static(string tooltipText)
    //{
    //    Instance.ShowTooltip(tooltipText);
    //}
    //public static void ShowTooltip_Static(System.Func<string> getTooltipTextFunc)
    //{
    //    Instance.ShowTooltip(getTooltipTextFunc);
    //}

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }
}
