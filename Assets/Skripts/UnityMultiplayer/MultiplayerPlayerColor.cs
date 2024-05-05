using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MultiplayerPlayerColor : MonoBehaviour
{
    [SerializeField] private ulong charOwnerClientId;

    public void SettingPlayerColor()
    {
        charOwnerClientId = transform.parent.GetComponent<StuffManagerScript>().myClientId;
        SetChosenColor();
    }

    private void SetChosenColor()
    {
        SetChosenColorServerRpc(transform.parent.GetComponent<NetworkObject>(), charOwnerClientId);          
    }

    [ServerRpc]
    private void SetChosenColorServerRpc(NetworkObjectReference myPlayerRef, ulong myCharOwnerClientId)
    {
        myPlayerRef.TryGet(out NetworkObject myPlayer);

        if (myPlayer == null)
        {
            return;
        }

        int colorId = MultiplayerGroupManager.MyInstance.GetPlayerDataFromClientId(myCharOwnerClientId).colorId;
        Color myColor = MultiplayerGroupManager.MyInstance.GetPlayerColor(colorId);
        Material material = new Material(myPlayer.transform.Find("PlayerAnimation").Find("Human.male_elegantsuit01").GetComponent<SkinnedMeshRenderer>().material);

        float emissiveIntensity = 0.3f;

        material.color = myColor;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", myColor * emissiveIntensity);

        myPlayer.transform.Find("PlayerAnimation").Find("Human.male_elegantsuit01").GetComponent<SkinnedMeshRenderer>().material = material;

        SetChosenColorClientRpc(myPlayerRef, myCharOwnerClientId);

    }

    [ClientRpc]
    private void SetChosenColorClientRpc(NetworkObjectReference myPlayerRef, ulong myCharOwnerClientId)
    {
        myPlayerRef.TryGet(out NetworkObject myPlayer);

        if (myPlayer == null)
        {
            return;
        }

        int colorId = MultiplayerGroupManager.MyInstance.GetPlayerDataFromClientId(myCharOwnerClientId).colorId;
        Color myColor = MultiplayerGroupManager.MyInstance.GetPlayerColor(colorId);
        Material material = new Material(myPlayer.transform.Find("PlayerAnimation").Find("Human.male_elegantsuit01").GetComponent<SkinnedMeshRenderer>().material);

        float emissiveIntensity = 0.3f;

        material.color = myColor;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", myColor * emissiveIntensity);

        myPlayer.transform.Find("PlayerAnimation").Find("Human.male_elegantsuit01").GetComponent<SkinnedMeshRenderer>().material = material;
    }
}