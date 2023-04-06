using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Talent : MonoBehaviour
{
    Button button;
    Image sprite;

    public GameObject PLAYER;
    public PlayerStats statSkript;
    TalentTree myTalentTree;

    [SerializeField]
    private int maxCount;

    public int currentCount = 0;

    private TextMeshProUGUI talentPointTextOwn;

    private void Awake()
    {
        button = GetComponent<Button>();
        sprite = GetComponent<Image>();
        button.onClick.AddListener(OnTalentButtonClick);
        talentPointTextOwn = transform.Find("Image").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        myTalentTree = transform.parent.parent.parent.parent.GetComponent<TalentTree>();
        //PLAYER = transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject;
        if (maxCount == 0)
        {
            maxCount = 5;
        }
    }

    void OnTalentButtonClick()
    {
        myTalentTree.TryUseTalent(this);
    }

    private void Start()
    {
        //statSkript = PLAYER.GetComponent<PlayerStats>();
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

    public void Lock()
    {
        button.interactable = false;
        sprite.color = Color.grey;
    }

    public void Unlock()
    {
        button.interactable = true;
        sprite.color = Color.white;
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
