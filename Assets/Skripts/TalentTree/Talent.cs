using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Talent : MonoBehaviour
{
    public string talentName;
    Button button;
    Image sprite;
    public string talentDescription = "";
    public string predecessor = " - ";
    public GameObject myPredecessorTalent;

    public GameObject PLAYER;
    public PlayerStats statSkript;
    TalentTree myTalentTree;

    [SerializeField] public int maxCount;
    [SerializeField] public int pointCost=1;

    public int currentCount = 0;

    private TextMeshProUGUI talentPointTextOwn;

    protected virtual void Awake()
    {
        UpdateTalent();
        button.onClick.AddListener(OnTalentButtonClick);
    }

    public virtual void UpdateTalent()
    {
        PLAYER = transform.parent.parent.parent.parent.parent.parent.parent.parent.gameObject;
        button = GetComponent<Button>();
        statSkript = PLAYER.GetComponent<PlayerStats>();
        sprite = transform.Find("TalentImage").GetComponent<Image>();
        talentPointTextOwn = transform.Find("Image").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        myTalentTree = transform.parent.parent.parent.parent.parent.parent.GetComponent<TalentTree>();
        //PLAYER = transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject;
        if (maxCount == 0) maxCount = 5;
        FindMyPredecessor();
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
        if (currentCount == maxCount)
        {
            Color32 myNewColor = transform.Find("Background").GetComponent<Image>().color;
            myNewColor.a = 255;
            transform.Find("Background").GetComponent<Image>().color = myNewColor;
        }

        if (currentCount < maxCount)
        {
            currentCount++;
            UpdatePointCounterAndBackground();
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
        if (button == null) button = GetComponent<Button>();
        if (sprite == null) sprite = transform.Find("TalentImage").GetComponent<Image>();

        button.interactable = false;
        Color32 newColor = sprite.color;
        newColor.a = 175;
        sprite.color = newColor;

        //sprite.color = Color.grey;
    }

    public void Unlock()
    {
        if (button == null) button = GetComponent<Button>();
        if (sprite == null) sprite = transform.Find("TalentImage").GetComponent<Image>();

        button.interactable = true;
        Color32 newColor = sprite.color;
        newColor.a = 255;
        sprite.color = newColor;
        //sprite.color = Color.white;
    }

    public void UpdatePointCounterAndBackground()
    {
        if (talentPointTextOwn == null) talentPointTextOwn = transform.Find("Image").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        if (currentCount == maxCount)
        {
            Color32 myNewColor = transform.Find("Background").GetComponent<Image>().color;
            myNewColor.a = 255;
            transform.Find("Background").GetComponent<Image>().color = myNewColor;
        }

        talentPointTextOwn.text = currentCount.ToString() + " / " + maxCount.ToString();
    }

    public virtual void ActiveTalentEffect()
    {
        GetComponent<MasterEventTriggerTalent>().GetTalentInfo();
    }

    public virtual void RemoveActiveTalentEffect()
    {
        GetComponent<MasterEventTriggerTalent>().GetTalentInfo();
    }

    public virtual void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        GetComponent<MasterEventTriggerTalent>().GetTalentInfo();
    }

    public virtual void FindMyPredecessor()
    {

    }
}
