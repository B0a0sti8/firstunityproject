using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TooltipScreenSpaceUIStats : MonoBehaviour
{
    public static TooltipScreenSpaceUIStats Instance { get; private set; }


    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform playerRectTransform;

    RectTransform backgroundRectTransform;
    RectTransform rectTransform;

    RectTransform statDescription;

    TextMeshProUGUI textMeshProDescription;

    void Awake()
    {
        Instance = this;

        backgroundRectTransform = transform.Find("Tooltip Background").GetComponent<RectTransform>();
        rectTransform = transform.GetComponent<RectTransform>();

        statDescription = transform.Find("Tooltip Text Description").GetComponent<RectTransform>();

        textMeshProDescription = transform.Find("Tooltip Text Description").GetComponent<TextMeshProUGUI>();

        HideTooltip();
    }

    void SetTooltip(string tooltipDescription)
    {
        textMeshProDescription.SetText(tooltipDescription);

        textMeshProDescription.ForceMeshUpdate();

        //Vector2 descriptionSize = textMeshProDescription.GetRenderedValues(true);
        //Vector2 baseHeight = new Vector2(0, descriptionSize.y);
        //Vector2 baseWidth = new Vector2(0, descriptionSize.x);
        //float xValue = 8f;
        //float yValue = 6f;
        //backgroundRectTransform.sizeDelta = baseHeight + baseWidth + new Vector2(2 * xValue, 2 * yValue - 4);
        //itemName.position = rectTransform.anchoredPosition + new Vector2(xValue, yValue + descriptionSize.y);
        //itemDescription.position = rectTransform.anchoredPosition + new Vector2(xValue, yValue);
        //image.transform.position = rectTransform.anchoredPosition + new Vector2(xValue, yValue);
    }

    void Update()
    {
        //float scaleAdjust = canvasRectTransform.localScale.x * playerRectTransform.localScale.x;
        //Vector2 anchoredPosition = Mouse.current.position.ReadValue() / scaleAdjust;

        //if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        //{
        //    // Tooltip left screen on right side
        //    anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        //}

        //if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        //{
        //    // Tooltip left screen on top side
        //    anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        //}

        //rectTransform.anchoredPosition = anchoredPosition;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }

    private void ShowTooltip(string tooltipDescription)
    {
        gameObject.SetActive(true); 
        SetTooltip(tooltipDescription);
    }

    public static void ShowTooltip_Static(string tooltipDescription)
    {
        Instance.ShowTooltip(tooltipDescription);
    }
}
