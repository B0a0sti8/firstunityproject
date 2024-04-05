using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CanvasGroupHealthScript : NetworkBehaviour
{
    public List<NetworkObject> allPlayerObjects;
    NetworkObject Player;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //MultiplayerGroupManager.MyInstance.CalltoClientConnectedServerRPC(OwnerClientId);
    }

    // Start is called before the first frame update
    void Start()
    {
        allPlayerObjects = new List<NetworkObject>();
        Player = transform.parent.parent.GetComponent<NetworkObject>();
    }

    // Update is called once per frame
    void Update()
    {
        FetchAllPlayerObjects();
    }

    public void FetchAllPlayerObjects()
    {
        if (!Player.IsOwner) { return; }

        GameObject[] pT1 = GameObject.FindGameObjectsWithTag("Player");
        
    }
}
