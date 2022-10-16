using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by: Brackeys - SAVE & LOAD SYSTEM in Unity

// no MonoBehaviour because this is not gonna act as a component in our game
//[System.Serializable] // be able to save it in a file
//public class PlayerData
//{
//    public int maxHealth;
//    public int currentHealth;
//    public float[] position; // instead of: public Vector3 position

//    // Constructor (acts as setup-functions for the class)
//    public PlayerData (Player player) // "Player" = name of Skript
//    {
//        //maxHealth = player.maxHealth;
//        //currentHealth = player.currentHealth;

//        position = new float[3];
//        position[0] = player.transform.position.x;
//        position[1] = player.transform.position.y;
//        position[2] = player.transform.position.z; // probably delete later
//    }
//}
