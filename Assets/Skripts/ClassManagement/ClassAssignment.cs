using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAssignment : MonoBehaviour
{
    GameObject PLAYER;
    PlayerStats playerStats;

    public void Awake()
    {
        PLAYER = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        playerStats = PLAYER.GetComponent<PlayerStats>();
        //skillBook = PLAYER.transform.Find("Own Canvases").transform.Find("Canvas Skillbook").transform.Find("Skillbook");
    }

    public void ChangeClassBlueMage()
    { playerStats.className = "BlueMage"; }

    public void ChangeClassWarrior()
    { playerStats.className = "Warrior"; }

    public void ChangeClassHunter()
    { playerStats.className = "Hunter"; }

    public void ChangeClassPriest()
    { playerStats.className = "Priest"; }

    public void ChangeClassMage()
    { playerStats.className = "Mage"; }

    public void ChangeClassPaladin()
    { playerStats.className = "Paladin"; }


    public void DeactivateClass()
    {

    }
}
