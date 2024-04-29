using UnityEngine;
using UnityEngine.InputSystem;

public class TalentTreeUI : MonoBehaviour
{
    public GameObject talentTreeUI;

    void Start()
    {
        talentTreeUI = transform.Find("TalentTreeWindow").gameObject;
        //talentTreeUI = gameObject;
        talentTreeUI.SetActive(false);
    }

    public void OpenTalentTree()
    {
        Debug.Log("Talent Tree An/Aus");
        talentTreeUI.SetActive(!talentTreeUI.activeSelf);
    }
}