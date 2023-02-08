using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    private Lobby hostLobby;
    float heartBeatTimer;


    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed In " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 10;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
            hostLobby = lobby; 

            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions       // Hier sind ein paar Beispiele für Einstellungen bei der Lobby-Suche:
            {
                Count = 25,                                                 // Stellt nur 25 Lobbies dar, auch wenn es evtl. mehr gibt.
                Filters = new List<QueryFilter>                             // Erlaubt es nach bestimmten Lobbies zu filtern
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)           // Hier zum Beispiel: Nur Lobbies mit mehr als 0 Available Slots (GT = Greater Than)
                },

                Order = new List<QueryOrder>                    // Gibt Regeln für die Reihenfolge der gefundenen Lobbies vor.
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)              // False: Ascending oder Descending? -> In dem Fall Descending (Absteigend) -> Also wird hier von alt nach neu sortiert.
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);                // Hier werden die Lobbies gesucht mit den obrigen Einstellungen
            Debug.Log("Lobbies found: " + queryResponse.Results);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);             // Mach für jede gefundene Lobby einen Debug.Log mit dem Namen und der maximalen Anzahl der Spieler
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
        
    }


    void Update()
    {
        HandleLobbyHeartbeat();
    }

    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartBeatTimer -= Time.deltaTime;
            if (heartBeatTimer < 0f)
            {
                float heartBeatTimerMax = 15;
                heartBeatTimer = heartBeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }
}
