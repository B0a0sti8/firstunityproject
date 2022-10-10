using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    [SerializeField] private Item item;
    [SerializeField] private float dropChance;

    public Item MyItem { get => item;}
    public float MyDropChance { get => dropChance; }
}
