using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 1)]
public class HealthPotion : Item
{
    GameObject[] PLAYERs;
    public override void Use()
    {
        PLAYERs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in PLAYERs)
        {
            if (player.GetComponent<PhotonView>().IsMine == true)
            {
                Debug.Log("HealthPotion war sehr effektiv!");
                player.GetComponent<PlayerStats>().GetHealing(20, 0, 0, 0);
            }
        }

        base.Use();
        Remove();
    }
}
