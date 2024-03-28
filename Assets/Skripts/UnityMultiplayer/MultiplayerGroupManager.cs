using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

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
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {


        MultiplayerPlayerData mulDa = new MultiplayerPlayerData { clientId = clientId };
        multiplayerPlayerDatas.Add(mulDa);
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        Debug.Log("Anzahl verbundener Spieler: " + multiplayerPlayerDatas.Count);
        return playerIndex < multiplayerPlayerDatas.Count;
    }

}
