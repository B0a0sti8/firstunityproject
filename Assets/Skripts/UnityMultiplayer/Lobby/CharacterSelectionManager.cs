using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterSelectionManager : NetworkBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }
    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
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
            Debug.Log("Alle Spieler bereit. Starte Spiel!");
        }

    }
}
