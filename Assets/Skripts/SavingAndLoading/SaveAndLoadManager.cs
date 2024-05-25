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
    [SerializeField] private Button loadButton;
    private ulong myClientId;

    private void Awake()
    {
        saveButton.onClick.AddListener(() => { SaveCurrentState(); });
        loadButton.onClick.AddListener(() => { LoadCharacter(); });
    }

    public void SetClientId(ulong clientId)
    {
        //myClientId = clientId;
    }

    private void SaveCurrentState()
    {
        if (MultiplayerGroupManager.MyInstance == null)
        {
            GameObject.Find("PLAYER(Clone)").transform.Find("GameManager").GetComponent<PlayerSaveLoad>().Save("TestChar".ToString());
            return;
        }
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

    private void LoadCharacter()
    {
        if (MultiplayerGroupManager.MyInstance != null)
        {
            LoadCharacterServerRpc();
        }
        else
        {
            Transform myPlayer = GameObject.Find("PLAYER(Clone)").transform;
            StartCoroutine(WaitForCharacterToSpawn("TestChar", myPlayer));
        }
    }

    [ServerRpc]
    public void LoadCharacterServerRpc()
    {
        LoadCharacterClientRpc();
    }

    [ClientRpc]
    public void LoadCharacterClientRpc()
    {
        MultiplayerPlayerData mulPlaDa = MultiplayerGroupManager.MyInstance.GetPlayerDataFromClientId(myClientId);
        FixedString128Bytes myCharacterName = mulPlaDa.characterName;
        mulPlaDa.playerObject.TryGet(out NetworkObject myPlayerObject);
        StartCoroutine(WaitForCharacterToSpawn(myCharacterName.ToString(), myPlayerObject.transform));
    }

    IEnumerator WaitForCharacterToSpawn(string characterName, Transform myPlayerObject)
    {
        yield return new WaitForSeconds(0.3f);
        myPlayerObject.Find("GameManager").GetComponent<PlayerSaveLoad>().Load(characterName.ToString());
    }
}
