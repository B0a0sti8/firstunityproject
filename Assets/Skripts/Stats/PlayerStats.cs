using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    // Start is called before the first frame update
    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;

    }

	void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
	{
		if (newItem != null)
		{
			health.AddModifierAdd(newItem.healthModifierAdd);
			armor.AddModifierAdd(newItem.armorModifierAdd);
			damage.AddModifierAdd(newItem.damageModifierAdd);
			evade.AddModifierAdd(newItem.evadeModifierAdd);

			health.AddModifierMultiply(newItem.healthModifierMultiply);
			armor.AddModifierMultiply(newItem.armorModifierMultiply);
			damage.AddModifierMultiply(newItem.damageModifierMultiply);
			evade.AddModifierMultiply(newItem.evadeModifierMultiply);
		}

		if (oldItem != null)
		{
			health.RemoveModifierAdd(oldItem.healthModifierAdd);
			armor.RemoveModifierAdd(oldItem.armorModifierAdd);
			damage.RemoveModifierAdd(oldItem.armorModifierAdd);
			evade.RemoveModifierAdd(oldItem.evadeModifierAdd);

			health.RemoveModifierMultiply(oldItem.healthModifierMultiply);
			armor.RemoveModifierMultiply(oldItem.armorModifierMultiply);
			damage.RemoveModifierMultiply(oldItem.armorModifierMultiply);
			evade.RemoveModifierMultiply(oldItem.evadeModifierMultiply);
		}

	}
}
