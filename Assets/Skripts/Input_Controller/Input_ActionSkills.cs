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
    { skillSlots.transform.Find("ActionSkill11").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill12() 
    { skillSlots.transform.Find("ActionSkill12").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill13()
    { skillSlots.transform.Find("ActionSkill13").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill14()
    { skillSlots.transform.Find("ActionSkill14").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill15()
    { skillSlots.transform.Find("ActionSkill15").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill16()
    { skillSlots.transform.Find("ActionSkill16").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill17()
    { skillSlots.transform.Find("ActionSkill17").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill18()
    { skillSlots.transform.Find("ActionSkill18").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill19()
    { skillSlots.transform.Find("ActionSkill19").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill20()
    { skillSlots.transform.Find("ActionSkill20").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill21()
    { skillSlots.transform.Find("ActionSkill21").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill22()
    { skillSlots.transform.Find("ActionSkill22").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill23()
    { skillSlots.transform.Find("ActionSkill23").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill24()
    { skillSlots.transform.Find("ActionSkill24").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill25()
    { skillSlots.transform.Find("ActionSkill25").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill26()
    { skillSlots.transform.Find("ActionSkill26").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill27()
    { skillSlots.transform.Find("ActionSkill27").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill28()
    { skillSlots.transform.Find("ActionSkill28").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill29()
    { skillSlots.transform.Find("ActionSkill29").GetComponent<ActionButton>().UseSkillOnClick(); }

    void OnActionSkill30()
    { skillSlots.transform.Find("ActionSkill30").GetComponent<ActionButton>().UseSkillOnClick(); }
}