using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsKannNurEinenGeben : MonoBehaviour
{
    public static EsKannNurEinenGeben instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; // make sure no more code is called, before destroying the object
        }
    }
}