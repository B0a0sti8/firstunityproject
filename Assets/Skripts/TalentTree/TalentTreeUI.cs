using UnityEngine;
using UnityEngine.InputSystem;

public class TalentTreeUI : MonoBehaviour
{
    public GameObject talentTreeUI;

    void Start()
    {
        talentTreeUI = gameObject.transform.Find("TalentTrees").gameObject;
        talentTreeUI.SetActive(false);
    }

    public void OpenTalentTree()
    {
        Debug.Log("Talent Tree An/Aus");
        talentTreeUI.SetActive(!talentTreeUI.activeSelf);
    }
}