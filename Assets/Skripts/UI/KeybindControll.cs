using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindControll : MonoBehaviour
{
    static KeybindControll instance;

    public static KeybindControll MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindControll>();
            }
            return instance;
        }
    }

    [SerializeField] 
    CanvasGroup keybindMenue;

    GameObject[] keybindButtons;

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    public void OpenCloseMenue()
    {
        keybindMenue.alpha = keybindMenue.alpha > 0 ? 0 : 1; // if > 0 -> 0, else 1
        keybindMenue.blocksRaycasts = keybindMenue.blocksRaycasts == true ? false : true; 
    }

    private void OnKeybindMenue()
    {
        OpenCloseMenue();
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        Debug.Log(keybindButtons[0]);
        Debug.Log(keybindButtons[0].name);
        Debug.Log(key);
        Debug.Log(key == keybindButtons[0].name);
        TMPro.TextMeshProUGUI tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<TMPro.TextMeshProUGUI>();
        Debug.Log(tmp);
        Debug.Log(code);
        tmp.text = code.ToString();
    }
}
