using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffSlot : MonoBehaviour
{
    Buff buff;
    public Image icon;
    TextMeshProUGUI timeText;
    public string buffName;
    public string buffDescription;
    MasterEventTriggerBuffs masterETBuffs;

    public void AddBuff(Buff newBuff)
    {
        buff = newBuff;             // Neuer Buff wird in den Slot gepackt
        icon.sprite = buff.icon;    // Icon wird aktualisiert
        icon.enabled = true;        // Icon wird angezeigt
    }

    public void ClearSlot()
    {
        buff = null;            // Item wird entfernt
        icon.sprite = null;     // Icon gelöscht
        icon.enabled = false;   // Kein Icon angezeigt
    }

    void Start()
    {
        timeText = transform.Find("Text Buff Duration").GetComponent<TextMeshProUGUI>();

        masterETBuffs = GetComponent<MasterEventTriggerBuffs>();
    }

    void Update()
    {
        if (buff == null)
        {
            timeText.text = "";
            return;
        }
        
        if (buff.durationTimeLeft >= 0)
        {
            timeText.text = Mathf.Round(buff.durationTimeLeft).ToString();
        }
        
        MasterETStuffAssignment();
    }

    void MasterETStuffAssignment()
    {
        masterETBuffs.buffName = buffName;

        masterETBuffs.buffDescription = buffDescription;

        //masterETBuffs.buffSprite = Resources.Load<Sprite>("Sprites/BuffSprites/" + buffName);
    }


    // Kann später verwendet werden um Buffs wegzuklicken, falls man sie nicht haben will.
    //public void UseItem()
    //{
    //    if (buff != null)
    //    {
    //        buff.Use(); // Wenn Button gedrückt und Item vorhanden, verwende Item
    //    }
    //}
}