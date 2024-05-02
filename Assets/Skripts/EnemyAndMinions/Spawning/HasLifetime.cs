 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HasLifetime : NetworkBehaviour
{
    public float maxLifetime;
    public float startingTime;

    void Start()
    {
        startingTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;

        startingTime += Time.deltaTime;
        if (startingTime >= maxLifetime)
        {
            MinionPetAI petAI = GetComponent<MinionPetAI>();
            if (petAI != null)
            {
                petAI.Dying();

                RemoveOwnerServerRpc();
            }
            StartCoroutine(DelayedDestroy());
        }
    }

    [ServerRpc]
    void RemoveOwnerServerRpc()
    {
        GetComponent<MinionPetAI>().myMaster.GetComponent<PlayerStats>().myMinions.Remove(this.gameObject);
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        //NetworkObject.Despawn(this);
        gameObject.GetComponent<NetworkObject>().Despawn();
        //GameObject.Destroy(gameObject);
        
    }
}
