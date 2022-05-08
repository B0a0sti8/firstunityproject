using UnityEngine;
using UnityEngine.InputSystem;

public class ClassChoiceUI : MonoBehaviour
{
    public GameObject classChoiceUI;

    void Start()
    {
        classChoiceUI = gameObject.transform.Find("ClassChoice").gameObject;
    }

    public void OpenClassChoice()
    {
        Debug.Log("Class Choice An/Aus");
        classChoiceUI.SetActive(!classChoiceUI.activeSelf);
    }

    public void ApplyClass()
    {
        classChoiceUI.SetActive(!classChoiceUI.activeSelf);
    }
}