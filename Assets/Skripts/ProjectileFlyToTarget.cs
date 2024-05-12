using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.Netcode;

public class ProjectileFlyToTarget : MonoBehaviour
{
    public Transform target;
    public float timeToArrive;
    public bool isDestroyedWhenTargetHit;

    private void Start()
    {
        isDestroyedWhenTargetHit = true;
    }

    void Update()
    {
        //if (!IsServer)
        //{ return; }

        if (timeToArrive > 0)
        {
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position);
                transform.position += direction * Time.deltaTime / timeToArrive;

                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.back);
                }
            }
           
            timeToArrive -= Time.deltaTime;
        }
        else
        {
            //NetworkObject.Despawn(this);
            if (isDestroyedWhenTargetHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
