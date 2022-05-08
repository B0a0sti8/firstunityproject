using UnityEngine;
using UnityEngine.InputSystem;

public class TalentTreeUI : MonoBehaviour
{
    public GameObject talentTreeUI;

    void Start()
    {
        talentTreeUI = gameObject.transform.Find("TalentTree").gameObject;
    }

    public void OpenTalentTree()
    {
        Debug.Log("Class Choice An/Aus");
        talentTreeUI.SetActive(!talentTreeUI.activeSelf);
    }
}