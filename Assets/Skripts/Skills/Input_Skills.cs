using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Skills : MonoBehaviour
{
    public TestSkill testSkill;
    private void OnTestSkill()
    {
        testSkill.StartSkillChecks();
    }

    public TestSpeedBoost testSpeedBoost;
    private void OnTestSpeedBoost()
    {
        testSpeedBoost.StartSkillChecks();
    }

    public TestGainLife testGainLife;
    private void OnTestGainLife()
    {
        testGainLife.StartSkillChecks();
    }

    public TestGainLifeHot testGainLifeHot;
    private void OnTestGainLifeHot()
    {
        testGainLifeHot.StartSkillChecks();
    }

    public TestAttack1 testAttack1;
    private void OnTestAttack1()
    {
        testAttack1.StartSkillChecks();
    }

    public TestAttack2 testAttack2;
    private void OnTestAttack2()
    {
        testAttack2.StartSkillChecks();
    }

    public TestAttack3 testAttack3;
    private void OnTestAttack3()
    {
        testAttack3.StartSkillChecks();
    }

    public TestOverHeal testOverHeal;
    private void OnTestOverHeal()
    {
        testOverHeal.StartSkillChecks();
    }

    public TestBigAttack testBigAttack;
    private void OnTestBigAttack()
    {
        testBigAttack.StartSkillChecks();
    }

    public TestEnergyDrain testEnergyDrain;
    private void OnTestEnergyDrain()
    {
        testEnergyDrain.StartSkillChecks();
    }
}

//public _____ _____;
//private void On_____()
//{
//    _____.StartSkillChecks();
//}