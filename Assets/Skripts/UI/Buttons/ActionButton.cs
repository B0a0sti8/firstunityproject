using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    public Button MyButton { get; private set; }
    GameObject PLAYER;
    GameObject skillManager;
    public string skillName;
    public string className;



    void Start()
    {
        PLAYER = transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
        skillManager = PLAYER.transform.Find("SkillManager").gameObject;
        MyButton = GetComponent<Button>();
    }

    public void UseSkillOnClick()
    {
        if (className == null) return;

        SkillPrefab[] skills = skillManager.transform.Find("BlueMage").GetComponents<SkillPrefab>();

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].GetType().ToString() == skillName)
            {
                skills[i].StartSkillChecks();
            }
        }

        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Wenn Button geklickt wird ausgeführt (nur Mausklick)
        UseSkillOnClick();
    }
}

//private IUseable useable;
//if(useable != null)
//{
//    useable.Use();
//}