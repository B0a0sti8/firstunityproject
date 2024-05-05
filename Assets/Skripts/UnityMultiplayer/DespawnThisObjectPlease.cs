using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DespawnThisObjectPlease : NetworkBehaviour
{
    public void DespawnThisObjectP()
    {
        if (!IsServer)
        {
            return;
        }

        StartCoroutine(DespawnMainMinionTimer());
    }

    public IEnumerator DespawnMainMinionTimer()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
