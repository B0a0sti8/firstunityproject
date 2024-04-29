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
            }
            StartCoroutine(DelayedDestroy());
        }
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        //NetworkObject.Despawn(this);
        GameObject.Destroy(gameObject);
    }
}
