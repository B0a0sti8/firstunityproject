using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Collections;

public delegate void KillConfirmed(CharacterStats characterStats);
public class StuffManagerScript : NetworkBehaviour
{
    public event KillConfirmed killConfirmedEvent;

    [SerializeField] private GameObject messagePrefab;

    [SerializeField] public ulong myClientId;
    [SerializeField] private NetworkVariable<FixedString128Bytes> characterName;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


        SetCharacterName((FixedString128Bytes)"DefaultName"); // Für den Fall dass es keinen MultiplayerGroupManager gibt (z.B. SinglePlayerTests)
        Debug.Log(characterName.Value);
        myClientId = OwnerClientId;

        if (MultiplayerGroupManager.MyInstance != null)
        {
            SetCharacterName(MultiplayerGroupManager.MyInstance.GetPlayerDataFromClientId(myClientId).characterName);
            Debug.Log("MyCharacterName: ");
            Debug.Log(characterName.Value);
            transform.Find("PlayerAnimation").GetComponent<MultiplayerPlayerColor>().SettingPlayerColor();
            transform.Find("Canvas World Space").GetComponent<PlayerNameWorldSpaceUI>().ShowPlayerName();

            MultiplayerGroupManager.MyInstance.AddPlayerObjectToList(myClientId, gameObject.GetComponent<NetworkObject>());
        }

        if (SaveAndLoadManager.MyInstance != null)
        {
            SaveAndLoadManager.MyInstance.SetClientId(myClientId);
        }

    }

    public void OnKillConfirmed(CharacterStats characterStats)
    {
        if (killConfirmedEvent != null)
        {
            killConfirmedEvent(characterStats);
        }
    }

    public void WriteMessage(string message)
    {
        Debug.Log("MessageWrite");
        GameObject go = Instantiate(messagePrefab, transform.Find("Own Canvases").Find("CanvasQuestUI").Find("MessageFeed"));
        go.GetComponent<TextMeshProUGUI>().text = message;

        go.transform.SetAsFirstSibling();

        Destroy(go, 2);
    }

    public void SetCharacterName(FixedString128Bytes newCharacterName)
    {
        SetCharacterNameServerRpc(newCharacterName);
    }

    [ServerRpc]
    public void SetCharacterNameServerRpc(FixedString128Bytes newCharacterName)
    {
        characterName.Value = newCharacterName;
    }

    public FixedString128Bytes GetCharacterName()
    {
        Debug.Log("Returning Character Name.");
        return characterName.Value;
    }
}
