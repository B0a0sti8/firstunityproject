using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;



[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 1)]
public class HealthPotion : Item
{
    GameObject[] PLAYERs;

    [SerializeField]
    private int healing;
    public override void Use()
    {
        PLAYERs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in PLAYERs)
        {
            if (player.GetComponent<NetworkBehaviour>().IsOwner == true)
            {
                Debug.Log("HealthPotion war sehr effektiv!");
                DamageOrHealing.DoHealing(null, player.GetComponent<NetworkBehaviour>(), healing);
                //player.GetComponent<PlayerStats>().GetHealing(healing, 0, 0, 0);
            }
        }

        base.Use();
        Remove();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Awake()
    {
        base.Awake();
        tooltipItemName = "Health Potion";
        tooltipItemDescription = "Heals for 15";
    }
}
