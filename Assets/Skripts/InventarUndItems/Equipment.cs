using UnityEngine;

/* An Item that can be equipped to increase armor/damage. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class Equipment : Item
{

	public EquipmentSlot equipSlot;

	public float healthModifierAdd;
	public float armorModifierAdd;
	public float damageModifierAdd;
	public float evadeModifierAdd;

	public float healthModifierMultiply;
	public float armorModifierMultiply;
	public float damageModifierMultiply;
	public float evadeModifierMultiply;

	// Called when pressed in the inventory
	public override void Use()
	{
		EquipmentManager.instance.Equip(this);
		RemoveFromInventory();
	}

}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, Shield, Feet }
