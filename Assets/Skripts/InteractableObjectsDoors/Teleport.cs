using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class Teleport : Interactable
{
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Ich interagiere");

        LeaveTavern();
     
    }

    public void LeaveTavern()
    {
        NetworkManager.SceneManager.LoadScene("0_Town", LoadSceneMode.Single);
    }
}
