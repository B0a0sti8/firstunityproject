using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageChest : Interactable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite openSprite, closedSprite;

    private bool isOpen;

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
        }
    }

    public override void StopInteracting()
    {
        base.StopInteracting();
        spriteRenderer.sprite = closedSprite;
        isOpen = false;
    }
}
