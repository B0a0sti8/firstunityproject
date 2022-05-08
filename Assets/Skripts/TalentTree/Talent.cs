using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Talent : MonoBehaviour
{
    private Button button;

    [SerializeField]
    private int maxCount;

    public int currentCount;

    private TextMeshProUGUI talentPointTextOwn;

    private void Awake()
    {
        button = GetComponent<Button>();
        talentPointTextOwn = gameObject.transform.Find("Image").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (maxCount == 0)
        {
            maxCount = 5;
        }
    }

    public void Lock()
    {
        button.interactable = false;
    }

    public bool TryAllocateTalent()
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            UpdatePointCounter();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Unlock()
    {
        button.interactable = true;
    }

    public void UpdatePointCounter()
    {
        talentPointTextOwn.text = currentCount.ToString() + " / " + maxCount.ToString();
    }
}
