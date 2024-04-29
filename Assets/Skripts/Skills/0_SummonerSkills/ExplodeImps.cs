using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ExplodeImps : SkillPrefab
{
    List<GameObject> myImps;
    bool isFlingingImps = false;
    float elapsedFlingTime = 0f;
    float maxFlingTime = 0.3f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        myClass = "Summoner";

        hasGlobalCooldown = true;

        needsTargetEnemy = true;
        skillRange = 20;
        skillRadius = 0.5f;

        hasOwnCooldown = true;
        ownCooldownTimeBase = 3f;

        castTimeOriginal = 1f;
        animationTime = 1f;
        isAOECircle = true;

        elapsedFlingTime = 0f;
        maxFlingTime = 0.1f;
    }

    public override void Update()
    {
        base.Update();
        if (isFlingingImps)
        {
            if (elapsedFlingTime >= maxFlingTime)
            {
                isFlingingImps = false;

                Debug.Log("Minions werden geworfen! Ziel: " + currentTargets.Count + currentTargets[0]);
                for (int i = myImps.Count - 1; i >= 0; i--)
                {
                    Debug.Log(i);
                    GameObject imp = myImps[i];
                    DealDamage(288);
                    RemoveImpFromListServerRpc(imp.GetComponent<NetworkObject>(), PLAYER, i);
                    DespawnImpServerRpc(imp.GetComponent<NetworkObject>());
                }

                return;
            }

            elapsedFlingTime += Time.deltaTime;

            if (myImps.Count != 0)
            {
                foreach (GameObject imp in myImps)
                {
                    Vector3 targetVector = mainTargetForCircleAoE.transform.position - imp.transform.position;
                    if (targetVector.magnitude >= 1)
                    {
                        FlingImpServerRpc(targetVector, imp);
                    }
                }
            }
        }
    }

    [ServerRpc]
    private void FlingImpServerRpc(Vector3 targetVector, NetworkObjectReference impRef)
    {
        impRef.TryGet(out NetworkObject imp);
        if (imp != null)
        {
            imp.transform.position += targetVector * elapsedFlingTime / maxFlingTime;
            imp.GetComponent<MinionPetMovement>().speed = 0;
        }
    }

    [ServerRpc]
    private void SetMinionOutOfFightServerRpc(NetworkObjectReference impRef)
    {
        impRef.TryGet(out NetworkObject imp);
        if (imp != null)
        {
            imp.GetComponent<MinionPetAI>().isInFight = false;
        }
    }

    [ServerRpc]
    private void DespawnImpServerRpc(NetworkObjectReference impRef)
    {
        impRef.TryGet(out NetworkObject imp);
        if (imp != null)
        {
            imp.GetComponent<HasLifetime>().startingTime = imp.GetComponent<HasLifetime>().maxLifetime;
        }
    }

    [ServerRpc]
    private void RemoveImpFromListServerRpc(NetworkObjectReference impRef, NetworkObjectReference playerRef, int impIndex)
    {
        impRef.TryGet(out NetworkObject imp);
        playerRef.TryGet(out NetworkObject myPlayer);

        if (imp != null && myPlayer != null)
        {
            imp.gameObject.SetActive(false);
            myPlayer.GetComponent<PlayerStats>().myMinions.RemoveAt(impIndex);
            RemoveImpFromListClientRpc(impRef, playerRef, impIndex);
        }
    }

    [ClientRpc]
    private void RemoveImpFromListClientRpc(NetworkObjectReference impRef, NetworkObjectReference playerRef, int impIndex)
    {
        if (IsHost)
        {
            return;
        }

        impRef.TryGet(out NetworkObject imp);
        playerRef.TryGet(out NetworkObject myPlayer);

        if (imp != null && myPlayer != null)
        {
            myPlayer.GetComponent<PlayerStats>().myMinions.RemoveAt(impIndex);
        }
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        myImps = PLAYER.GetComponent<PlayerStats>().myMinions;
        foreach (GameObject imp in myImps)
        {
            SetMinionOutOfFightServerRpc(imp);
        }
        isFlingingImps = true;
    }
}