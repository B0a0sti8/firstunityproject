using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class CharacterSelectionManager : NetworkBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }
    private Dictionary<ulong, bool> playerReadyDictionary;
    public event EventHandler OnReadyChanged;
    private Transform managerParent;

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
        managerParent = GameObject.Find("Managers").transform;
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                // Ein Spieler ist noch nicht bereit! Oder hat noch nicht abgestimmt.
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            // Hier könnte das Spiel dann gestartet werden. Jeder hat seinen Charakter gewählt.
            // Die Geladene Scene hängt vermutlich vom Story-Fortschritt ab?
            Debug.Log("Alle Spieler bereit. Starte Spiel!");

            if (managerParent.Find("LobbyManager") != null)
            {
                Destroy(managerParent.Find("LobbyManager").gameObject);
            }

            if (managerParent.Find("LobbyAssets") != null)
            {
                Destroy(managerParent.Find("LobbyAssets").gameObject);
            }

            if (managerParent.Find("CharacterSelectionManager") != null)
            {
                Destroy(managerParent.Find("CharacterSelectionManager").gameObject);
            }

            DestroyManagerSystemsClientRpc();
            NetworkManager.SceneManager.LoadScene("Tavern_New", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId)
    {
        playerReadyDictionary[clientId] = true;
        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    [ClientRpc]
    private void DestroyManagerSystemsClientRpc()
    {
        if (managerParent.Find("LobbyManager") != null)
        {
            Destroy(managerParent.Find("LobbyManager").gameObject);
        }

        if (managerParent.Find("LobbyAssets") != null)
        {
            Destroy(managerParent.Find("LobbyAssets").gameObject);
        }

        if (managerParent.Find("CharacterSelectionManager") != null)
        {
            Destroy(managerParent.Find("CharacterSelectionManager").gameObject);
        }
    }

    public bool IsPlayerReady(ulong clientId)
    {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }
}
