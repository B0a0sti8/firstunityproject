using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// inspired by: Code Monkey - Dynamic Tooltip in Unity! (Resizable, Follows Mouse, Edge Detection)

public class TooltipScreenSpaceUIBuffs : MonoBehaviour
{
    public static TooltipScreenSpaceUIBuffs Instance { get; private set; }


    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform playerRectTransform;

    RectTransform backgroundRectTransform;
    RectTransform rectTransform;

    RectTransform buffName;
    RectTransform buffDescription;

    TextMeshProUGUI textMeshProName;
    TextMeshProUGUI textMeshProDescription;


    void Awake()
    {
        Instance = this;

        backgroundRectTransform = transform.Find("Tooltip Background").GetComponent<RectTransform>();
        rectTransform = transform.GetComponent<RectTransform>();

        buffName = transform.Find("Tooltip Text Buffname").GetComponent<RectTransform>();
        buffDescription = transform.Find("Tooltip Text Description").GetComponent<RectTransform>();

        textMeshProName = transform.Find("Tooltip Text Buffname").GetComponent<TextMeshProUGUI>();
        textMeshProDescription = transform.Find("Tooltip Text Description").GetComponent<TextMeshProUGUI>();

        HideTooltip();
    }

    void SetTooltip(string tooltipName, string tooltipDescription)
    {
        textMeshProName.SetText(tooltipName);
        textMeshProDescription.SetText(tooltipDescription);

        textMeshProName.ForceMeshUpdate();
        textMeshProDescription.ForceMeshUpdate();

        Vector2 nameSize = textMeshProName.GetRenderedValues(true);
        Vector2 descriptionSize = textMeshProDescription.GetRenderedValues(true);
        Vector2 baseHeight = new Vector2 (0, nameSize.y + descriptionSize.y);
        Vector2 baseWidth = new Vector2(0, 0);
        if (nameSize.x > descriptionSize.x)
        { baseWidth.x = nameSize.x; }
        else
        { baseWidth.x = descriptionSize.x; }
        float xValue = 8f;
        float yValue = 6f;
        backgroundRectTransform.sizeDelta = baseHeight + baseWidth + new Vector2(2 * xValue, 2 * yValue - 4);
        buffName.position = rectTransform.anchoredPosition + new Vector2(xValue, yValue + descriptionSize.y);
        buffDescription.position = rectTransform.anchoredPosition + new Vector2(xValue, yValue);
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

    private void ShowTooltip(string tooltipName, string tooltipDescription)
    {
        gameObject.SetActive(true); // maybe switch rows :3
        SetTooltip(tooltipName, tooltipDescription);
    }

    public static void ShowTooltip_Static(string tooltipName, string tooltipDescription)
    {
        Instance.ShowTooltip(tooltipName, tooltipDescription);
    }
}
