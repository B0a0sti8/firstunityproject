using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.InputSystem.Editor;
#endif


#region Eigentliches Skript
/*
using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.InputSystem.Utilities;



public class Input_ActionSkills : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    private void Awake()
    {
        //playerInput.actions["ActionSkill11"].AddCompositeBinding("ActionSkill11").With("Modifier1", "<Keyboard>/leftCtrl").With("Modifier1", "<Keyboard>/rightCtrl").With("Button", "<Keyboard>/1");
    }

    void OnActionSkill1()
    { }

    void OnActionSkill2()
    { }

    void OnActionSkill3()
    { }

    void OnActionSkill4()
    { }

    void OnActionSkill5()
    { }

    void OnActionSkill6()
    { }

    void OnActionSkill7()
    { }

    void OnActionSkill8()
    { }

    void OnActionSkill9()
    { }

    void OnActionSkill10()
    { }

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
*/
#endregion






#region Trash
/*
#region Trash

[InitializeOnLoad]
[DisplayStringFormat("{multiplier}*{stick}")]
public class CustomComposite1 : InputBindingComposite<Vector2>
{
    static CustomComposite1()
    {
        Initialize();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    private static void Initialize()
    {
        InputSystem.RegisterBindingComposite<CustomComposite>();
    }

    [InputControl(layout = "Axis")]
    public int multiplier;

    [InputControl(layout = "Vector2")]
    public int stick;

    public float scaleFactor = 1;

    public override Vector2 ReadValue(ref InputBindingCompositeContext context)
    {
        var stickValue = context.ReadValue<Vector2, Vector2MagnitudeComparer>(stick);
        var multiplierValue = context.ReadValue<float>(multiplier);
        return stickValue * (multiplierValue * scaleFactor);
    }
}

public class CustomCompositeEditor1 : InputParameterEditor<CustomComposite>
{
    public override void OnGUI()
    {
        var currentValue = target.scaleFactor;
        target.scaleFactor = EditorGUILayout.Slider(m_ScaleFactorLabel, currentValue, 0, 2);
    }
    private GUIContent m_ScaleFactorLabel = new GUIContent("Scale Factor");
}
#endregion



#region TestPlatz2
/*
[Preserve]
[DisplayStringFormat("{modifier}+{button}")]
public class ActionSkillButton11 : InputBindingComposite<float>
{
    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        throw new System.NotImplementedException();
    }

    void Awake()
    {
        var action = new InputAction(type: InputActionType.Button);
        action.AddCompositeBinding("ActionSkill11")
            .With("Modifier", "<Keyboard>/leftCtrl")
            .With("Modifier", "<Keyboard>/rightControl")
            .With("Button", "<Keyboard>/1");
    }
}
#endregion
*/

#endregion