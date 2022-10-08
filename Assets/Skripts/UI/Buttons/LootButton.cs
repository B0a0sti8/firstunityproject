using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text title;

    public Image MyIcon { get => icon; set => icon = value; }
    public Text MyTitle { get => title; set => title = value; }
}
