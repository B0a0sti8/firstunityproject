using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    #region Singleton
    static KeybindManager instance;

    public static KeybindManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
            }
            return instance;
        }
    }
    #endregion

    public Dictionary<string, KeyCode> Keybinds { get; private set; } // W A S D

    public Dictionary<string, KeyCode> ActionBinds { get; private set; } // 1 2 3 ...

    string bindName;

    [SerializeField]
    CanvasGroup keybindMenue;

    GameObject[] keybindButtons;

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    void Start()
    {
        keybindMenue.alpha = 0;
        keybindMenue.blocksRaycasts = false;

        Keybinds = new Dictionary<string, KeyCode>();

        ActionBinds = new Dictionary<string, KeyCode>();

        BindKey("Up", KeyCode.W);
        BindKey("Left", KeyCode.A);
        BindKey("Down", KeyCode.S);
        BindKey("Right", KeyCode.D);

        BindKey("Action 1", KeyCode.Q);
        BindKey("Action 2", KeyCode.E);
        BindKey("Action 3", KeyCode.Alpha1);
        BindKey("Action 4", KeyCode.Alpha2);
        BindKey("Action 5", KeyCode.Alpha3);
        BindKey("Action 6", KeyCode.Alpha4);
        BindKey("Action 7", KeyCode.Alpha5);
        BindKey("Action 8", KeyCode.Alpha6);
        BindKey("Action 9", KeyCode.Alpha7);
        BindKey("Action 10", KeyCode.Alpha8);
        BindKey("Action 11", KeyCode.Alpha9);
        BindKey("Action 12", KeyCode.Alpha0);
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = Keybinds;

        if (key.Contains("Action"))
        {
            currentDictionary = ActionBinds;
        }

        if (!currentDictionary.ContainsValue(keyBind))
        {
            currentDictionary.Add(key, keyBind);
            KeybindManager.MyInstance.UpdateKeyText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind))
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            currentDictionary[myKey] = KeyCode.None;
            KeybindManager.MyInstance.UpdateKeyText(key, KeyCode.None);
        }

        currentDictionary[key] = keyBind;
        KeybindManager.MyInstance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        TMPro.TextMeshProUGUI tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<TMPro.TextMeshProUGUI>();
        tmp.text = code.ToString();
    }

    public void OpenCloseMenue()
    {
        keybindMenue.alpha = keybindMenue.alpha > 0 ? 0 : 1; // if > 0 -> 0, else 1
        keybindMenue.blocksRaycasts = keybindMenue.blocksRaycasts == true ? false : true;
    }
}