using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RemoveAllMainMinions : SkillPrefab
{
    SummonerClass mySummonerClass;
    List<GameObject> mainMinionsToBeRemoved = new List<GameObject>();

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        myClass = "Summoner";

        hasOwnCooldown = false;
        hasGlobalCooldown = false;

        needsTargetEnemy = false;

        castTimeOriginal = 0;
        animationTime = 0f;
        isAOECircle = false;

        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    [ServerRpc]
    private void SetMainMinionsOutOfFightServerRpc(NetworkObjectReference minionRef)
    {
        minionRef.TryGet(out NetworkObject minio);
        if (minio != null)
        {
            minio.GetComponent<MinionPetAI>().isInFight = false;
        }
    }

    [ServerRpc]
    private void DespawnMainMinionServerRpc(NetworkObjectReference minionRef)
    {
        minionRef.TryGet(out NetworkObject minio);

        //minio.GetComponent<DespawnThisObjectPlease>().DespawnThisObjectP();
        Destroy(minio.gameObject);
        return;
    }

    [ServerRpc]
    private void RemoveMainNinionFromListServerRpc(NetworkObjectReference minionRef, NetworkObjectReference playerRef, int minionIndex)
    {
        minionRef.TryGet(out NetworkObject minio);
        playerRef.TryGet(out NetworkObject myPlayer);

        if (minio != null && myPlayer != null)
        {
            myPlayer.GetComponent<PlayerStats>().myMainMinions.RemoveAt(minionIndex);
        }
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        mainMinionsToBeRemoved.Clear();

        foreach (var mainMin in PLAYER.GetComponent<PlayerStats>().myMainMinions)
        {
            mainMin.TryGet(out NetworkObject mainMi);
            if (mainMi != null)
            {
                mainMinionsToBeRemoved.Add(mainMi.gameObject);
            }
        }

        for (int i = mainMinionsToBeRemoved.Count - 1; i >= 0; i--)
        {
            Debug.Log(i);
            GameObject minion = mainMinionsToBeRemoved[i];
            SetMainMinionsOutOfFightServerRpc(minion);
            RemoveMainNinionFromListServerRpc(minion.GetComponent<NetworkObject>(), PLAYER, i);
            DespawnMainMinionServerRpc(minion);
        }
    }


}
