using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using QFSW.QC;

public class MultiplayerGroupManager : NetworkBehaviour
{
    private static MultiplayerGroupManager instance;

    public static MultiplayerGroupManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MultiplayerGroupManager>();
            }
            return instance;
        }
    }

    [SerializeField] private NetworkList<MultiplayerPlayerData> multiplayerPlayerDatas;

    public event EventHandler OnMultiplayerPlayerDatasChanged;

    [SerializeField] private List<Color> playerColorList;

    [SerializeField] GameObject playerNameTextField;
    private FixedString128Bytes playerNameSet;

    [SerializeField] private Transform Player_Prefab;

    private string previousSceneName;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }

        previousSceneName = SceneManager.GetActiveScene().name;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        multiplayerPlayerDatas = new NetworkList<MultiplayerPlayerData>();
        multiplayerPlayerDatas.OnListChanged += MultiplayerPlayerDatas_OnListChanged;
    }

    private void MultiplayerPlayerDatas_OnListChanged(NetworkListEvent<MultiplayerPlayerData> changeEvent)
    {
        OnMultiplayerPlayerDatasChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
        for (int i = 0; i < multiplayerPlayerDatas.Count; i++)
        {
            MultiplayerPlayerData mulPlaDa = multiplayerPlayerDatas[i];
            if (mulPlaDa.clientId == clientId)
            {
                // Dieser Client hat die Verbindung aufgegeben
                multiplayerPlayerDatas.RemoveAt(i);
            }
        }
    }

    private void NetworkManager_Client_OnClientConnectedCallback(ulong clientId)
    {
        SetNewNetworkList(clientId);

        //string[] conversionText = playerNameTextField.GetComponent<TextMeshProUGUI>().text.Split(": ");
        //FixedString128Bytes thisPlayerName = new FixedString128Bytes(conversionText[1]);

        //MultiplayerPlayerData mulDa = new MultiplayerPlayerData { clientId = clientId, colorId = GetFirstUnusedColorId(), playerName = playerNameSet };
        //multiplayerPlayerDatas.Add(mulDa);
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < multiplayerPlayerDatas.Count;
    }

    public MultiplayerPlayerData GetPlayerDataFromClientId(ulong clientId)
    {
        foreach (MultiplayerPlayerData mulPlaDa in multiplayerPlayerDatas)
        {
            if (mulPlaDa.clientId == clientId)
            {
                return mulPlaDa;
            }
        }

        return default;
    }

    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < multiplayerPlayerDatas.Count; i++)
        {
            if (multiplayerPlayerDatas[i].clientId == clientId)
            {
                return i;
            }
        }
        return -1;
    }

    public MultiplayerPlayerData GetPlayerData()
    {
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }

    public MultiplayerPlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        return multiplayerPlayerDatas[playerIndex];
    }

    public Color GetPlayerColor(int colorId)
    {
        return playerColorList[colorId];
    }

    public void ChangePlayerColor(int colorId)
    {
        ChangePlayerColorServerRpc(colorId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangePlayerColorServerRpc(int colorId, ServerRpcParams serverRpcParams = default)
    {
        if (!IsColorAvailable(colorId))
        {
            // Color not available
            return;
        }

        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        MultiplayerPlayerData mulPlaDa = multiplayerPlayerDatas[playerDataIndex];

        mulPlaDa.colorId = colorId;
        multiplayerPlayerDatas[playerDataIndex] = mulPlaDa;
    }

    private bool IsColorAvailable(int colorId)
    {
        foreach (MultiplayerPlayerData mulPlayDa in multiplayerPlayerDatas)
        {
            if (mulPlayDa.colorId == colorId)
            {
                // Already in use
                return false;
            }
        }
        return true;
    }

    private int GetFirstUnusedColorId()
    {
        for (int i = 0; i < playerColorList.Count; i++)
        {
            if (IsColorAvailable(i))
            {
                return i;
            }
        }
        return -1;
    }

    public void KickPlayer(ulong clientId)
    {
        NetworkManager_Server_OnClientDisconnectCallback(clientId);
        NetworkManager.Singleton.DisconnectClient(clientId);
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (previousSceneName == "StartLobby")
        {
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                Transform playerTransform = Instantiate(Player_Prefab);
                playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, false);
            }
        }
        previousSceneName = sceneName;
    }

    public void AddPlayerObjectToList(ulong clientId, NetworkObject player)
    {
        Debug.Log("Lokal: Versuche SpielerCharakter in Liste aufzunehmen.");
        //NetworkObjectReference playerReference = player.GetComponent<NetworkObjectReference>();
        NetworkObjectReference playerReference = (NetworkObjectReference)player;
        AddPlayerObjectToListServerRpc(clientId, playerReference);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddPlayerObjectToListServerRpc(ulong clientId, NetworkObjectReference Player)
    {
        Debug.Log("ServerRpc: Versuche SpielerCharakter in Liste aufzunehmen.");
        MultiplayerPlayerData mulDaPla = GetPlayerDataFromClientId(clientId);

        mulDaPla.playerObject = Player;

        int index = GetPlayerDataIndexFromClientId(clientId);
        multiplayerPlayerDatas[index] = mulDaPla;
    }

    public void SetNewNetworkList(ulong clientId)
    {
        FixedString128Bytes characterName = new FixedString128Bytes("");
        SetNewNetworkListServerRpc(clientId, characterName);
    }

    public void SetNewNetworkListString(string charName)
    {
        ulong myClientId = NetworkManager.Singleton.LocalClientId;

        string[] conversionText = playerNameTextField.GetComponent<TextMeshProUGUI>().text.Split(": ");
        FixedString128Bytes thisPlayerName = new FixedString128Bytes(conversionText[1]);

        int myColorId = GetFirstUnusedColorId();

        for (int i = 0; i < multiplayerPlayerDatas.Count; i++)
        {
            if (multiplayerPlayerDatas[i].clientId == myClientId)
            {
                myColorId = multiplayerPlayerDatas[i].colorId;
            }
        }
        SetNewNetworkListStringServerRpc(myClientId, myColorId, thisPlayerName, charName);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetNewNetworkListStringServerRpc(ulong clientId, int colorId, FixedString128Bytes playerName, FixedString128Bytes charName)
    {
        bool hasFoundElement = false;
        for (int i = 0; i < multiplayerPlayerDatas.Count; i++)
        {
            if (multiplayerPlayerDatas[i].clientId == clientId)
            {
                MultiplayerPlayerData mulDa = new MultiplayerPlayerData { clientId = clientId, colorId = colorId, playerName = playerName, characterName = charName };
                multiplayerPlayerDatas[i] = mulDa;
                hasFoundElement = true;
            }
        }

        if (!hasFoundElement)
        {
            MultiplayerPlayerData mulDa = new MultiplayerPlayerData { clientId = clientId, colorId = colorId, playerName = playerName, characterName = charName };
            multiplayerPlayerDatas.Add(mulDa);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetNewNetworkListServerRpc(ulong newClientId, FixedString128Bytes charName)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { newClientId }
            }
        };

       SetNewNetworkListClientRpc(newClientId, charName, clientRpcParams);
    }

    [ClientRpc]
    private void SetNewNetworkListClientRpc(ulong newClientId, FixedString128Bytes charName, ClientRpcParams clientRpcParams = default)
    {
        string[] conversionText = playerNameTextField.GetComponent<TextMeshProUGUI>().text.Split(": ");
        FixedString128Bytes thisPlayerName = new FixedString128Bytes(conversionText[1]);
        ReturnNetWorkListServerRpc(newClientId, thisPlayerName, charName);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ReturnNetWorkListServerRpc(ulong newClientId, FixedString128Bytes thisPlayerName, FixedString128Bytes charName)
    {
        MultiplayerPlayerData mulDa = new MultiplayerPlayerData { clientId = newClientId, colorId = GetFirstUnusedColorId(), playerName = thisPlayerName, characterName = charName };
        multiplayerPlayerDatas.Add(mulDa);
    }

    [Command]
    private void QuantDebugEntry(int num)
    {
        Debug.Log(multiplayerPlayerDatas[num].clientId);
        Debug.Log(multiplayerPlayerDatas[num].colorId);
        Debug.Log(multiplayerPlayerDatas[num].playerName);
        Debug.Log(multiplayerPlayerDatas[num].characterName);
        bool hasFoundObject = multiplayerPlayerDatas[num].playerObject.TryGet(out NetworkObject playerObjectFromReference);
        Debug.Log(hasFoundObject);
        Debug.Log(playerObjectFromReference);
    }

    [Command]
    private void QuantDebugCount()
    {
        Debug.Log(multiplayerPlayerDatas.Count);
    }
}