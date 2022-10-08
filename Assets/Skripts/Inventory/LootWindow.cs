using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootWindow : MonoBehaviour
{

    [SerializeField] private LootButton[] lootButtons;
    [SerializeField] private Item[] items;

    private static Dictionary<ItemQuality, string> nameColors = new Dictionary<ItemQuality, string>()
    {{ ItemQuality.Common, "#ECEBEB8A"},{ ItemQuality.Uncommon, "#05CB198A"},{ ItemQuality.Rare, "#141FC38A"},{ ItemQuality.Epic, "#B512A78A"},{ ItemQuality.Mythic, "#FDB9478A"}};

    private static Dictionary<ItemQuality, string> MyNameColors
    { get { return nameColors; } }

    // Start is called before the first frame update
    void Start()
    {
        AddLoot();
    }

    private void AddLoot()
    {
        int itemIndex = 0;

        string title = string.Format("<color={0}>{1}</color>", MyNameColors[items[itemIndex].itemQuality], items[itemIndex].name);
        lootButtons[itemIndex].MyIcon.sprite = items[itemIndex].MyIcon;
        lootButtons[itemIndex].gameObject.SetActive(true);
        lootButtons[itemIndex].MyTitle.text = title;
    }
}
