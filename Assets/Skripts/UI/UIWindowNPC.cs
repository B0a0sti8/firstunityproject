using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowNPC : MonoBehaviour
{
    [SerializeField] protected CanvasGroup canvasGroup;

    private NPC npc;

    public virtual void Open(NPC npc)
    {
        this.npc = npc;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

    }

    public virtual void Close()
    {
        npc.IsInteracting = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        npc = null;
    }


}
