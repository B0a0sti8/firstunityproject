using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TMPro;

public class PlayerNameWorldSpaceUI : MonoBehaviour
{
    private FixedString128Bytes playerName;

    private void Start()
    {
        ShowPlayerName();
    }

    public void ShowPlayerName()
    {
        playerName = transform.parent.GetComponent<StuffManagerScript>().GetCharacterName().Value;
        transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>().text = playerName.ToString();
    }
}
