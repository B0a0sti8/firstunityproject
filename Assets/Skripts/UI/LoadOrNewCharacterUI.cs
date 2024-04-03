using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadOrNewCharacterUI : MonoBehaviour
{
    private Button applyButton;
    [SerializeField] private Button playerReadyButton;

    private void Awake()
    {
        applyButton = transform.Find("ApplyButton").GetComponent<Button>();
        applyButton.onClick.AddListener(() => { Hide(); playerReadyButton.interactable = true; });
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
