using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

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

    public List<GameObject> playerList;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        playerList = new List<GameObject>();

    }

    [ServerRpc(RequireOwnership = false)]
    public void CalltoClientConnectedServerRPC(ulong id)
    {
        ulong playerObj = NetworkManager.Singleton.ConnectedClients[id].PlayerObject.NetworkObjectId;
        ClientConnectedClientRPC(playerObj);
    }

    [ClientRpc]
    private void ClientConnectedClientRPC(ulong playerObjectID)
    {
        print("Hallo ich bin da!");
       // playerList.Add()
    }

    public void FetchAllClientIDs()
    {

    }
}
