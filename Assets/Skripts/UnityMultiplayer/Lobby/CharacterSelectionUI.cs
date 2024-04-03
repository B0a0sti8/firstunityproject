using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;
    [SerializeField] private Button changeCharacterButton;
    [SerializeField] private GameObject loadOrNewCharacterWindow;

    private void Awake()
    {
        readyButton.interactable = false;
        readyButton.onClick.AddListener(() => { CharacterSelectionManager.Instance.SetPlayerReady(); });
        changeCharacterButton.onClick.AddListener(() => { readyButton.interactable = false; loadOrNewCharacterWindow.SetActive(true); });
    }
}
