using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSkills : MonoBehaviour
{
    public ActionSkills actionSkills;

    public ActionSkill_BigAttack actionSkill_BigAttack;

    public ActionSkill_EnergyDrain actionSkill_EnergyDrain;

    private void OnSpeedBoost()
    {
        actionSkills.SpeedBoost();
    }

    private void OnGainLife()
    {
        actionSkills.GainLife();
    }

    private void OnGainLifeHot()
    {
        actionSkills.GainLifeHot();
    }

    private void OnOverHeal()
    {
        actionSkills.OverHeal();
    }

    private void OnEnergyDrain()
    {
        actionSkill_EnergyDrain.EnergyDrain();
    }

    private void OnBigAttack()
    {
        actionSkill_BigAttack.BigAttack();
    }

    private void OnAttack1()
    {
        actionSkills.Attack1();
    }

    private void OnAttack2()
    {
        actionSkills.Attack2();
    }

    private void OnAttack3()
    {
        actionSkills.Attack3();
    }
}
