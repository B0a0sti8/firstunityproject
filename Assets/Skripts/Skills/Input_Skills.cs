using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Skills : MonoBehaviour
{
    public TestSkill testSkill;
    private void OnTestSkill()
    {
        testSkill.ConditionCheck();
    }
}
