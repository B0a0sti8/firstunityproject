using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoadManager : MonoBehaviour
{
    private static SaveAndLoadManager instance;
    public static SaveAndLoadManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveAndLoadManager>();
            }
            return instance;
        }
    }

    [SerializeField] private Button saveButton;
    private ulong myClientId;

    private void Awake()
    {
        saveButton.onClick.AddListener(() => { SaveCurrentState(); });
    }

    public void SetClientId(ulong clientId)
    {
        //myClientId = clientId;
    }

    private void SaveCurrentState()
    {
        Debug.Log("Speichern Step 1");
        myClientId = NetworkManager.Singleton.LocalClientId;

        MultiplayerPlayerData mulPlaDa = MultiplayerGroupManager.MyInstance.GetPlayerDataFromClientId(myClientId);
        FixedString128Bytes playerCharacterName = mulPlaDa.characterName;
        bool hasFetchedPlayer = mulPlaDa.playerObject.TryGet(out NetworkObject localPlayerObject);
        if (hasFetchedPlayer)
        {
            localPlayerObject.transform.Find("GameManager").GetComponent<PlayerSaveLoad>().Save(playerCharacterName.ToString());
        }
    }
}
