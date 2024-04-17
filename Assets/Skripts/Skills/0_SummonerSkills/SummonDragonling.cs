using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonDragonling : SkillPrefab
{
    [SerializeField] private GameObject dragonling;

    public override void Start()
    {
        base.Start();
        myClass = "Summoner";

        hasGlobalCooldown = true;

        isSelfCast = true;

        hasOwnCooldown = true;
        ownCooldownTimeBase = 3f;

        castTimeOriginal = 1f;
        isSkillChanneling = false;
    }

    public override void Update()
    {
        tooltipSkillDescription = "Summons a Dragonling that helps you destroy things.";

        base.Update();
    }

    public override void ConditionCheck()
    {
        //if (transform.GetComponent<SummonerClass>().myMainSummonerMinions.Count >= transform.GetComponent<SummonerClass>().maxNrOfMainSummonerMinions)
        //{ Debug.Log("Zu viele Summoner Begleiter!"); return; }

        if (playerStats.myMainMinions.Count >= playerStats.maxNrOfMainMinions)
        { Debug.Log("Zu viele Begleiter!"); return; }
        base.ConditionCheck();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        NetworkObjectReference playerReference = (NetworkObjectReference)PLAYER;
        Debug.Log("Summoning Dragonling");
        SpawnDragonlingServerRpc(playerReference);


    }

    
    [ServerRpc]
    private void SpawnDragonlingServerRpc(NetworkObjectReference summoningPlayer, ServerRpcParams serverRpcParams = default)
    {
        ulong sumPlaID = serverRpcParams.Receive.SenderClientId;
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { sumPlaID }
            }
        };

        Debug.Log("Summon Dragonling Server RPC!");
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        float x = Random.Range(2, 5);
        float y = Random.Range(2, 5);
        float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

        if (sumPla != null)
        {
            Vector2 posi = (Vector2)sumPla.transform.position + new Vector2(x * signx, y * signy);
            GameObject dragonl = Instantiate(dragonling, posi, Quaternion.identity);
            dragonl.GetComponent<NetworkObject>().Spawn();
            dragonl.GetComponent<MinionPetAI>().myMaster = sumPla.transform;

            sumPla.GetComponent<PlayerStats>().myMainMinions.Add(dragonl);
            //transform.GetComponent<SummonerClass>().myMainSummonerMinions.Add(dragonl);

            NetworkObjectReference dragonlingRef = (NetworkObjectReference)dragonl;
            SpawnDragonlingClientRpc(summoningPlayer, dragonlingRef);
            // SpawnDragonlingClientRpc(summoningPlayer, clientRpcParams);
        }

    }

    [ClientRpc]
    private void SpawnDragonlingClientRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference dragonling, ClientRpcParams clientRpcParams = default)
    {
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        dragonling.TryGet(out NetworkObject drag);
        GameObject drago = drag.gameObject;

        drago.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
        sumPla.GetComponent<PlayerStats>().myMainMinions.Add(drago);
    }

}
