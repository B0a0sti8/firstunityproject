using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Tipp für die Zukunft: Fall im Multiplayer Modus komische Sachen bei Storage Chests oder Vendors passieren:
// Vielleicht macht es Sinn, das öffnen und schließen des jeweiligen Interfaces (Skript: VendorWindow bzw. StorageChest) auf den PLAYER-Charakter selbst zu verschieben. 

public class VendorWindow : MonoBehaviour
{
    [SerializeField] public CanvasGroup canvasGroup;

    [SerializeField] private VendorButton[] vendorButtons;

    private List<List<VendorItem>> pages = new List<List<VendorItem>>();

    [SerializeField] private Text pagenumber;

    private int pageIndex;

    private Vendor vendor;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void CreatePages(VendorItem[] items)
    {
        pages.Clear();

        List<VendorItem> page = new List<VendorItem>();

        for (int i = 0; i < items.Length; i++)
        {
            page.Add(items[i]);

            if (page.Count == 10 || i == items.Length - 1)
            {
                pages.Add(page);
                page = new List<VendorItem>();
            }
        }

        AddItems();
    }

    public void AddItems()
    {
        pagenumber.text = pageIndex + 1 + " / " + pages.Count;

        if (pages.Count > 0)
        {
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    vendorButtons[i].AddItem(pages[pageIndex][i]);
                }
            }
        }
    }

    public void Open(Vendor vendor)
    {
        this.vendor = vendor;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        vendor.IsOpen = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        vendor = null;
    }

    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)
        {
            ClearButtons();
            pageIndex++;
            AddItems();
        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            ClearButtons();
            pageIndex--;
            AddItems();
        }
    }

    public void ClearButtons()
    {
        foreach (VendorButton btn in vendorButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }
}
