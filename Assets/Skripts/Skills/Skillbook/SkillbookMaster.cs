using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillbookMaster : MonoBehaviour
{
    GameObject skillbook;

    void Start()
    {
        skillbook = gameObject.transform.Find("Skillbook").gameObject;
        skillbook.SetActive(false);
    }

    public void OpenSkillbook()
    {
        if (skillbook.activeInHierarchy)
        {
            skillbook.SetActive(false);
        }
        else
        {
            skillbook.SetActive(true);
        }
    }
}
