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
        for (int i = 0; i < transform.childCount; i++)
        {
            int colorId = MultiplayerGroupManager.MyInstance.GetPlayerDataFromClientId(charOwnerClientId).colorId;
            Color myColor = MultiplayerGroupManager.MyInstance.GetPlayerColor(colorId);
            Material material = new Material(transform.GetChild(i).Find("Human.male_elegantsuit01").GetComponent<SkinnedMeshRenderer>().material);

            float emissiveIntensity = 0.3f;

            material.color = myColor;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", myColor * emissiveIntensity);

            transform.GetChild(i).Find("Human.male_elegantsuit01").GetComponent<SkinnedMeshRenderer>().material = material;
        }
    }

}
