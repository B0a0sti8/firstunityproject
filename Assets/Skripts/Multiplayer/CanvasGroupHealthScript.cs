using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CanvasGroupHealthScript : NetworkBehaviour
{
    public List<NetworkObject> allPlayerObjects;
    NetworkObject Player;
    GroupHealthCanvas_GroupMember[] myGroupMemberScripts;

    // Start is called before the first frame update
    void Start()
    {
        allPlayerObjects = new List<NetworkObject>();
        Player = transform.parent.parent.GetComponent<NetworkObject>();
        myGroupMemberScripts = GetComponentsInChildren<GroupHealthCanvas_GroupMember>();

        GetPlayersAndUpdateUI();
    }

    public void GetPlayersAndUpdateUI()
    {
        foreach (GroupHealthCanvas_GroupMember gm in myGroupMemberScripts)
        {
            gm.Hide();
        }

        if (MultiplayerGroupManager.MyInstance != null)
        {
            int playerCount = MultiplayerGroupManager.MyInstance.GetCurrentPlayerCount();

            for (int i = 0; i < playerCount; i++)
            {
                MultiplayerPlayerData playerData = MultiplayerGroupManager.MyInstance.GetPlayerDataFromPlayerIndex(i);
                myGroupMemberScripts[i].myPlayerObject = playerData.playerObject;
                myGroupMemberScripts[i].myPlayerName = playerData.characterName.ToString();
                myGroupMemberScripts[i].Show();
                myGroupMemberScripts[i].UpdateMyUI();
            }
        }
        else // Singleplayer zum Testen von Sachen
        {
            myGroupMemberScripts[0].myPlayerObject = transform.parent.parent.gameObject;
            myGroupMemberScripts[0].myPlayerName = "SinglePlayerTestName";
            myGroupMemberScripts[0].Show();
            myGroupMemberScripts[0].UpdateMyUI();

        }
    }
}
