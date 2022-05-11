using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Talent : MonoBehaviour
{
    private Button button;

    public GameObject PLAYER;
    public PlayerStats statSkript;

    [SerializeField]
    private int maxCount;

    public int currentCount;

    private TextMeshProUGUI talentPointTextOwn;

    private void Awake()
    {
        button = GetComponent<Button>();
        talentPointTextOwn = gameObject.transform.Find("Image").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        PLAYER = transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject;
        if (maxCount == 0)
        {
            maxCount = 5;
        }
    }

    private void Start()
    {
        statSkript = PLAYER.GetComponent<PlayerStats>();

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
            ActiveTalentEffect();
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

    public virtual void ActiveTalentEffect()
    {
        
    }

    public virtual void RemoveActiveTalentEffect()
    {

    }
}
