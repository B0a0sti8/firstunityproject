using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class PlayerNameWorldSpaceUI : MonoBehaviour
{
    private FixedString128Bytes playerName;
    private Transform PLAYER;

    private void Start()
    {
        //ShowPlayerName();
    }

    public void ShowPlayerName()
    {
        PLAYER = transform.parent;
        playerName = PLAYER.GetComponent<StuffManagerScript>().GetCharacterName().Value;
        ShowPlayerNameServerRpc(PLAYER.GetComponent<NetworkObject>(), playerName);
    }

    public void ShowPlayerNameWithString(FixedString128Bytes currentPlayerName)
    {
        PLAYER = transform.parent;
        //playerName = PLAYER.GetComponent<StuffManagerScript>().GetCharacterName().Value;
        ShowPlayerNameServerRpc(PLAYER.GetComponent<NetworkObject>(), currentPlayerName);
    }

    [ServerRpc]
    public void ShowPlayerNameServerRpc(NetworkObjectReference myPlayerRef, FixedString128Bytes myPlayerName)
    {
        myPlayerRef.TryGet(out NetworkObject myPlayer);

        if (myPlayer != null)
        {
            myPlayer.transform.Find("Canvas World Space").Find("PlayerNameText").GetComponent<TextMeshProUGUI>().text = myPlayerName.ToString();
            ShowPlayerNameClientRpc(myPlayerRef, myPlayerName);

            Debug.Log("ShowPlayerNameServerRpc");
            Debug.Log("ServerRPc PlayerName : " + myPlayerName.ToString());
        }
    }

    [ClientRpc]
    public void ShowPlayerNameClientRpc(NetworkObjectReference myPlayerRef, FixedString128Bytes myPlayerName)
    {
        myPlayerRef.TryGet(out NetworkObject myPlayer);

        if (myPlayer != null)
        {
            playerName = myPlayer.GetComponent<StuffManagerScript>().GetCharacterName().Value;
            myPlayer.transform.Find("Canvas World Space").Find("PlayerNameText").GetComponent<TextMeshProUGUI>().text = myPlayerName.ToString();

            Debug.Log("ShowPlayerNameClientRpc");
            Debug.Log("ClientRpc PlayerName : " + myPlayerName.ToString());
        }
    }
}
