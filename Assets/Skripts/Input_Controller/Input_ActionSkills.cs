using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_ActionSkills : MonoBehaviour
{
    GameObject PLAYER;
    GameObject skillSlots;

    void Awake()
    {
        PLAYER = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        skillSlots = PLAYER.transform.Find("Own Canvases").Find("Canvas Action Skills").Find("SkillSlots").gameObject;
    }

    void OnActionSkill1() // 1
    { skillSlots.transform.Find("ActionSkill1").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill2() // 2
    { skillSlots.transform.Find("ActionSkill2").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill3() // 3
    { skillSlots.transform.Find("ActionSkill3").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill4() // 4
    { skillSlots.transform.Find("ActionSkill4").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill5() // 5
    { skillSlots.transform.Find("ActionSkill5").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill6() // 6
    { skillSlots.transform.Find("ActionSkill6").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill7() // 7
    { skillSlots.transform.Find("ActionSkill7").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill8() // 8
    { skillSlots.transform.Find("ActionSkill8").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill9() // 9
    { skillSlots.transform.Find("ActionSkill9").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill10() // 0
    { skillSlots.transform.Find("ActionSkill10").GetComponent<ActionButton>().UseSkillOnClick(); }

    // von Haus aus unbelegt. werden nur benutzt wenn der Spieler sie selbst belegt

    void OnActionSkill11() 
    { }

    void OnActionSkill12() 
    { }

    void OnActionSkill13()
    { }

    void OnActionSkill14()
    { }

    void OnActionSkill15()
    { }

    void OnActionSkill16()
    { }

    void OnActionSkill17()
    { }

    void OnActionSkill18()
    { }

    void OnActionSkill19()
    { }

    void OnActionSkill20()
    { }

    void OnActionSkill21()
    { }

    void OnActionSkill22()
    { }

    void OnActionSkill23()
    { }

    void OnActionSkill24()
    { }

    void OnActionSkill25()
    { }

    void OnActionSkill26()
    { }

    void OnActionSkill27()
    { }

    void OnActionSkill28()
    { }

    void OnActionSkill29()
    { }

    void OnActionSkill30()
    { }
}