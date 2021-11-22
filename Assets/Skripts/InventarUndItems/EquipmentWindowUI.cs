using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentWindowUI : MonoBehaviour
{
    public Transform equipSlotParent;
    public GameObject equipmentWindowUI;

    EquipmentManager equipmentManager;
    EquipmentWindowSlot[] eSlots;

    // Start is called before the first frame update
    void Start()
    {
        equipmentManager = EquipmentManager.instance;
        equipmentManager.onEquipmentChanged += UpdateUI;
        eSlots = equipSlotParent.GetComponentsInChildren<EquipmentWindowSlot>();
        for (int i = 0; i < eSlots.Length; i++)
        {
            Debug.Log(eSlots[i]);
        }
    }
 
    private void OnEquipmentWindow(InputValue value)
    {
        Debug.Log("EquipmentWindow An/Aus");
        equipmentWindowUI.SetActive(!equipmentWindowUI.activeSelf);
    }

    void UpdateUI(Equipment newItem, Equipment oldItem)        //Updated das UI
    {
        Debug.Log("Updating UI, EquipmentWindow");
           for (int i = 0; i < eSlots.Length; i++)      // Geht alle Slots durch
           {
               if (equipmentManager.currentEquipment[i] != null)          // Solange die Zählvariable kleiner ist, als die Anzahl der Items im Inventar
               {
                   eSlots[i].AddItem(equipmentManager.currentEquipment[i]);   // Füge dem nächsten Slot das nächste Item hinzu
               }
               else                    // Wenn keine Items mehr übrig sind
               {
                   eSlots[i].ClearSlot();       // Mach die übrigen Slots leer.
               }
           }
    }
}
