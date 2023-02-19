using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tipp für die Zukunft: Fall im Multiplayer Modus komische Sachen bei Storage Chests oder Vendors passieren:
// Vielleicht macht es Sinn, das öffnen und schließen des jeweiligen Interfaces (Skript: VendorWindow bzw. StorageChest) auf den PLAYER-Charakter selbst zu verschieben. 

public class StorageChest : Interactable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite openSprite, closedSprite;

    private bool isOpen = false;

    [SerializeField] public CanvasGroup canvasGroup;

    private List<Item> items;

    public List<Item> MyItems { get => items; set => items = value; }

    private void Start()
    {
        spriteRenderer.sprite = closedSprite;
    }

    public override void Interact()
    {
        base.Interact();
        if (isOpen)
        {
            StopInteracting();
        }
        else
        {
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            Debug.Log("Sollte zu sehen sein");
        }
    }

    public override void StopInteracting()
    {
        base.StopInteracting();
        spriteRenderer.sprite = closedSprite;
        isOpen = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

    }
}
