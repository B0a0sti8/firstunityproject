using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{ 
    [SerializeField] private LootButton[] lootButtons;

    private CanvasGroup canvasGroup;

    private List<List<Item>> pages = new List<List<Item>>();

    private List<Item> droppedLoot = new List<Item>();

    private int pageIndex = 0;

    [SerializeField] private Text pageNumber;

    [SerializeField] private GameObject nextBtn, previousBtn;

    [SerializeField] private Item[] items; // Nur fürs Debugging

    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }



    private static Dictionary<ItemQuality, string> nameColors = new Dictionary<ItemQuality, string>()
    {{ ItemQuality.Common, "#ECEBEB8A"},{ ItemQuality.Uncommon, "#05CB198A"},{ ItemQuality.Rare, "#141FC38A"},{ ItemQuality.Epic, "#B512A78A"},{ ItemQuality.Mythic, "#FDB9478A"}};

    private static Dictionary<ItemQuality, string> MyNameColors
    { get { return nameColors; } }



    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void CreatePages(List<Item> items)
    {
        if (!IsOpen)
        {
            List<Item> page = new List<Item>();
            droppedLoot = items;

            for (int i = 0; i < items.Count; i++)
            {
                page.Add(items[i]);

                if (page.Count == 5 || i == items.Count - 1)
                {
                    pages.Add(page);
                    page = new List<Item>();
                }
            }

            AddLoot();

            Open();
        }
    }

    private void AddLoot()
    {
        if (pages.Count > 0)
        {
            pageNumber.text = pageIndex + 1 + " / " + pages.Count;
            previousBtn.SetActive(pageIndex > 0);
            nextBtn.SetActive(pages.Count > 1 && pageIndex < pages.Count);

            for (int itemIndex = 0; itemIndex < pages[pageIndex].Count; itemIndex++)
            {
                if (pages[pageIndex][itemIndex] != null)
                {
                    lootButtons[itemIndex].MyIcon.sprite = pages[pageIndex][itemIndex].MyIcon;
                    lootButtons[itemIndex].MyItem = pages[pageIndex][itemIndex];
                    lootButtons[itemIndex].gameObject.SetActive(true);

                    string title = string.Format("<color={0}>{1}</color>", MyNameColors[pages[pageIndex][itemIndex].itemQuality], pages[pageIndex][itemIndex].name);
                    lootButtons[itemIndex].MyTitle.text = title;
                }
            }
        }
    }

    public void ClearButtons()
    {
        foreach (LootButton btn in lootButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)
        {
            pageIndex++;
            ClearButtons();
            AddLoot();
        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }
    }

    public void TakeLoot(Item loot)
    {
        pages[pageIndex].Remove(loot);
        droppedLoot.Remove(loot);
        

        if (pages[pageIndex].Count == 0)
        {
            pages.Remove(pages[pageIndex]);

            if (pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }

            AddLoot();
        }

    }

    public void Close()
    {
        pages.Clear();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        ClearButtons();
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }


}
