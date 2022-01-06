using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillbookButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    string skillName;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Button clicked: " + skillName);
        //HandScript.MyInstance.TakeMovable(Skillbook.MyInstance.GetSkill(skillName));
    }
}
