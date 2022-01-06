using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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

    GameObject PLAYER;
    PlayerInput playerInputG; PlayerInput playerInputA;

    [SerializeField]
    CanvasGroup keybindMenue;

    [SerializeField]
    ActionButton[] actionButtons;

    GameObject[] keybindButtons;

    void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        PLAYER = gameObject.transform.parent.gameObject;
        playerInputG = PLAYER.transform.Find("Input_Controller").transform.Find("Input_Gameplay").GetComponent<PlayerInput>();
        playerInputA = PLAYER.transform.Find("Input_Controller").transform.Find("Input_ActionSkills").GetComponent<PlayerInput>();
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

        BindKey("ActionSkill1", KeyCode.Alpha1);
        BindKey("ActionSkill1", KeyCode.L);
        BindKey("ActionSkill2", KeyCode.Alpha2);
        BindKey("ActionSkill3", KeyCode.Alpha3);
        BindKey("ActionSkill4", KeyCode.Alpha4);
        BindKey("ActionSkill5", KeyCode.Alpha5);
        BindKey("ActionSkill6", KeyCode.Alpha6);
        BindKey("ActionSkill7", KeyCode.Alpha7);
        BindKey("ActionSkill8", KeyCode.Alpha8);
        BindKey("ActionSkill9", KeyCode.Alpha9);
        BindKey("ActionSkill10", KeyCode.Alpha0);
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = Keybinds;

        if (key.Contains("Action"))
        {
            currentDictionary = ActionBinds;
        }

        if (!currentDictionary.ContainsKey(key))
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
        OnActionSkillsChangedCallback(key, code.ToString());
    }

    public void OpenCloseMenue()
    {
        keybindMenue.alpha = keybindMenue.alpha > 0 ? 0 : 1; // if > 0 -> 0, else 1
        keybindMenue.blocksRaycasts = keybindMenue.blocksRaycasts == true ? false : true;
    }

    void OnActionSkillsChangedCallback(string actionName, string binding)
    {
        if (binding.Contains("Alph"))
        { 
            binding = binding.Remove(0,5); 
        }

        string bindingMod = "<Keyboard>/" + binding;

        if (actionName == "Up" || actionName == "Down" || actionName == "Right" || actionName == "Left")
        {
            playerInputG.currentActionMap.FindAction("Movement").ChangeCompositeBinding("WASD").NextPartBinding(actionName).WithPath(bindingMod);
        } 
        else
        {
            playerInputA.currentActionMap.FindAction(actionName).ApplyBindingOverride(bindingMod);
        }
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }

    private void OnGUI()
    {
        if (bindName != string.Empty)
        {
            Event e = Event.current;
            if(e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}