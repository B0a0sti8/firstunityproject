using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Collections;
using UnityEngine.UI;
using TMPro;

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

    private NetworkList<MultiplayerPlayerData> multiplayerPlayerDatas;

    public event EventHandler OnMultiplayerPlayerDatasChanged;

    [SerializeField] private List<Color> playerColorList;

    [SerializeField] GameObject playerNameTextField;

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
        string[] conversionText = playerNameTextField.GetComponent<TextMeshProUGUI>().text.Split(": ");
        FixedString128Bytes thisPlayerName = new FixedString128Bytes(conversionText[1]);
        MultiplayerPlayerData mulDa = new MultiplayerPlayerData { clientId = clientId, colorId = GetFirstUnusedColorId(), playerName = thisPlayerName };
        multiplayerPlayerDatas.Add(mulDa);
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        Debug.Log("Anzahl verbundener Spieler: " + multiplayerPlayerDatas.Count);
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

}
