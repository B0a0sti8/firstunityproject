using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelScript : MonoBehaviour
{
    #region Singleton
    private static CharacterPanelScript instance;

    public static CharacterPanelScript MyInstance
    {
        get 
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CharacterPanelScript>();
            }

            return instance;
        } 
    }
    #endregion


    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private CharPanelButtonScript helmet, shoulders, chest, cape, gloves, legs, boots, artefact, mainhand, offhand;

    

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
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

    public void EquipStuff(Equipment equipment)
    {
        switch (equipment.MyEquipmentType)
        {
            case EquipmentType.Helmet:
                helmet.EquipStuff(equipment);
                break;
            case EquipmentType.Shoulders:
                shoulders.EquipStuff(equipment);
                break;
            case EquipmentType.Chest:
                chest.EquipStuff(equipment);
                break;
            case EquipmentType.Cape:
                cape.EquipStuff(equipment);
                break;
            case EquipmentType.Gloves:
                gloves.EquipStuff(equipment);
                break;
            case EquipmentType.Legs:
                legs.EquipStuff(equipment);
                break;
            case EquipmentType.Boots:
                boots.EquipStuff(equipment);
                break;
            case EquipmentType.Artefact:
                artefact.EquipStuff(equipment);
                break;
            case EquipmentType.Mainhand:
                mainhand.EquipStuff(equipment);
                break;
            case EquipmentType.OffHand:
                offhand.EquipStuff(equipment);
                break;
            case EquipmentType.TwoHand:
                mainhand.EquipStuff(equipment);
                break;
        }
    }
}
