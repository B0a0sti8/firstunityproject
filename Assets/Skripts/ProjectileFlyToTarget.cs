using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.Netcode;

public class ProjectileFlyToTarget : MonoBehaviour
{
    public Transform target;
    public float timeToArrive;

    void Update()
    {
        //if (!IsServer)
        //{ return; }

        if (timeToArrive > 0)
        {
            Vector3 direction = (target.position - transform.position);
            transform.position += direction * Time.deltaTime / timeToArrive;
            transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.back);
            timeToArrive -= Time.deltaTime;
        }
        else
        {
            //NetworkObject.Despawn(this);
            Destroy(gameObject);
        }
    }
}
