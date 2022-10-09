using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelScript : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }
}
