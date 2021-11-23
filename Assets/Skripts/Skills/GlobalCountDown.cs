using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalCountDown : MonoBehaviour
{
    public float timeLeft;

    GameObject[] textGameObjects;

    void Awake()
    {
        textGameObjects = GameObject.FindGameObjectsWithTag("WeaponSkillCDText");
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            foreach (GameObject gObj in textGameObjects)
            {
                gObj.GetComponent<TMPro.TextMeshProUGUI>().text = (Mathf.Round(timeLeft * 10) / 10.0).ToString("0.0").Replace(",", ".");
            }
        }
        else
        {
            foreach (GameObject gObj in textGameObjects)
            {
                gObj.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            }
        }
    }
}

// FixedUpdate is used for being in-step with the physics engine,
// so anything that needs to be applied to a rigidbody should happen in FixedUpdate.

// Update, on the other hand, works independantly of the physics engine.
// This can be benificial if a user's framerate were to drop but you need a certain calculation
// to keep executing, like if you were updating a chat or voip client, you would want regular old update.