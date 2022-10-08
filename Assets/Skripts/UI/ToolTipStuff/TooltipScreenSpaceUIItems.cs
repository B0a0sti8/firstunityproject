using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TooltipScreenSpaceUIItems : MonoBehaviour
{
    public static TooltipScreenSpaceUIItems Instance { get; private set; }


    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform playerRectTransform;

    RectTransform backgroundRectTransform;
    RectTransform rectTransform;

    RectTransform itemName;
    RectTransform itemDescription;

    TextMeshProUGUI textMeshProName;
    TextMeshProUGUI textMeshProDescription;

    Image image;


    void Awake()
    {
        Instance = this;

        backgroundRectTransform = transform.Find("Tooltip Background").GetComponent<RectTransform>();
        rectTransform = transform.GetComponent<RectTransform>();

        itemName = transform.Find("Tooltip Text Itemname").GetComponent<RectTransform>();
        itemDescription = transform.Find("Tooltip Text Description").GetComponent<RectTransform>();

        textMeshProName = transform.Find("Tooltip Text Itemname").GetComponent<TextMeshProUGUI>();
        textMeshProDescription = transform.Find("Tooltip Text Description").GetComponent<TextMeshProUGUI>();

        image = transform.Find("Tooltip Image").GetComponent<Image>();

        HideTooltip();
    }

    void SetTooltip(string tooltipName, string tooltipDescription, Sprite tooltipSprite)
    {
        textMeshProName.SetText(tooltipName);
        textMeshProDescription.SetText(tooltipDescription);

        image.sprite = tooltipSprite;

        textMeshProName.ForceMeshUpdate();
        textMeshProDescription.ForceMeshUpdate();

        Vector2 nameSize = textMeshProName.GetRenderedValues(true);
        Vector2 descriptionSize = textMeshProDescription.GetRenderedValues(true);
        Vector2 baseHeight = new Vector2(0, nameSize.y + descriptionSize.y);
        Vector2 baseWidth = new Vector2(0, 0);
        if (nameSize.x > descriptionSize.x)
        { baseWidth.x = nameSize.x; }
        else
        { baseWidth.x = descriptionSize.x; }
        float xValue = 8f;
        float yValue = 6f;
        backgroundRectTransform.sizeDelta = baseHeight + baseWidth + new Vector2(2 * xValue, 2 * yValue - 4); 
        itemName.position = rectTransform.anchoredPosition + new Vector2(xValue, yValue + descriptionSize.y);
        itemDescription.position = rectTransform.anchoredPosition + new Vector2(xValue, yValue);
        image.transform.position = rectTransform.anchoredPosition + new Vector2(xValue, yValue);

    }

    void Update()
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

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }

    private void ShowTooltip(string tooltipName, string tooltipDescription, Sprite tooltipSprite)
    {
        gameObject.SetActive(true); // maybe switch rows :3
        SetTooltip(tooltipName, tooltipDescription, tooltipSprite);
    }

    public static void ShowTooltip_Static(string tooltipName, string tooltipDescription, Sprite tooltipSprite)
    {
        Instance.ShowTooltip(tooltipName, tooltipDescription, tooltipSprite);
    }
}
