using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// inspired by: Code Monkey - Dynamic Tooltip in Unity! (Resizable, Follows Mouse, Edge Detection)

public class TooltipScreenSpaceUITalent : MonoBehaviour
{
    public static TooltipScreenSpaceUITalent Instance { get; private set; }

    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform playerRectTransform;

    RectTransform backgroundRectTransform;
    RectTransform rectTransform;

    TextMeshProUGUI textMeshProName;
    TextMeshProUGUI textMeshProDescription;
    Image image;
    TextMeshProUGUI textMeshProTalentCost;
    TextMeshProUGUI textMeshProAlreadySkilled;
    TextMeshProUGUI textMeshProPredecessor;


    bool moveWithMouse = false;

    private void Awake()
    {
        Instance = this;

        backgroundRectTransform = transform.Find("Tooltip Background").GetComponent<RectTransform>();
        rectTransform = transform.GetComponent<RectTransform>();

        textMeshProName = backgroundRectTransform.Find("Tooltip Text TalentName").GetComponent<TextMeshProUGUI>();
        textMeshProDescription = backgroundRectTransform.Find("Tooltip Text Description").GetComponent<TextMeshProUGUI>();
        image = backgroundRectTransform.Find("Tooltip Image").GetComponent<Image>();
        textMeshProTalentCost = backgroundRectTransform.Find("ToolTip Text Talentcost").GetComponent<TextMeshProUGUI>();
        textMeshProAlreadySkilled = backgroundRectTransform.Find("Tooltip Text AlreadySkilled").GetComponent<TextMeshProUGUI>();
        textMeshProPredecessor = backgroundRectTransform.Find("Tooltip Text Predecessor").GetComponent<TextMeshProUGUI>();

        HideTooltip();
    }

    void SetTooltip(string tooltipName, string tooltipDescription, Sprite tooltipSprite, string tooltipCost,
        string tooltipSkilled, string tooltipPredecessor)
    {
        textMeshProName.SetText(tooltipName);
        textMeshProDescription.SetText(tooltipDescription);
        image.sprite = tooltipSprite;
        textMeshProTalentCost.SetText(tooltipCost);
        textMeshProAlreadySkilled.SetText(tooltipSkilled);
        textMeshProPredecessor.SetText(tooltipPredecessor);

        textMeshProName.ForceMeshUpdate();
        textMeshProDescription.ForceMeshUpdate();
        textMeshProTalentCost.ForceMeshUpdate();
        textMeshProAlreadySkilled.ForceMeshUpdate();
        textMeshProPredecessor.ForceMeshUpdate();

    }

    private void Update()
    {
        //SetText(getTooltipTextFunc());

        if (moveWithMouse)
        {
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
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }

    private void ShowTooltip(string tooltipName, string tooltipDescription, Sprite tooltipSprite, string tooltipCost,
        string tooltipSkilled, string tooltipPredecessor)
    {
        gameObject.SetActive(true);
        SetTooltip(tooltipName, tooltipDescription, tooltipSprite,  tooltipCost,
         tooltipSkilled,  tooltipPredecessor);
    }

    public static void ShowTooltip_Static(string tooltipName, string tooltipDescription, Sprite tooltipSprite, string tooltipCost,
        string tooltipSkilled, string tooltipPredecessor)
    {
        Instance.ShowTooltip(tooltipName, tooltipDescription, tooltipSprite,  tooltipCost,
         tooltipSkilled,  tooltipPredecessor);
    }
}



