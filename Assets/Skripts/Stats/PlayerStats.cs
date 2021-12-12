using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerStats : CharacterStats
{
	public int maxMana = 1000;
	public int currentMana;
	[HideInInspector]
	public ManaBar manaBar;

	float tickEveryXSeconds = 1f; // gain mana every 1 second
	float tickEveryXSecondsTimer = 0f;

	void Start()
	{
		currentHealth = maxHealth.GetValue();
		currentMana = maxMana;
		manaBar = gameObject.transform.Find("Canvas World Space").transform.GetChild(1).GetComponent<ManaBar>();

		isAlive = true;

		EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
	}

	private void Update()
	{
		// updates Bars on Canvas
		gameObject.transform.Find("Canvas World Space").transform.GetChild(0).GetComponent<HealthBar>().SetMaxHealth((int)maxHealth.GetValue());
		gameObject.transform.Find("Canvas World Space").transform.GetChild(0).GetComponent<HealthBar>().SetHealth((int)currentHealth);

		if (currentHealth > maxHealth.GetValue())
		{
			currentHealth = maxHealth.GetValue();
		}

		if (currentHealth < 0)
		{
			currentHealth = 0;
		}

		manaBar.SetMaxMana(maxMana);
		manaBar.SetMana(currentMana);
		if (currentMana > maxMana)
		{
			currentMana = maxMana;
		}
		if (currentMana < 0)
		{
			currentMana = 0;
		}

		tickEveryXSecondsTimer += Time.deltaTime;
		if (tickEveryXSecondsTimer >= tickEveryXSeconds)
		{
			tickEveryXSecondsTimer = 0f;
				GetComponent<PhotonView>().RPC("ManageMana", RpcTarget.All, 25);
		}
	}

	[PunRPC]
	public override void TakeDamage(float damage)
	{
		Debug.Log("Player takes damage " + damage);
		base.TakeDamage(damage);
		FindObjectOfType<AudioManager>().Play("Oof");
	}

	[PunRPC]
	public void ManageMana(int manaCost)
    {
		if (GetComponent<PhotonView>().IsMine)
		{
			Debug.Log("Meins");
			currentMana += manaCost;
		}
		else
		{
			Debug.Log("Nich so meins");
		}
	}


	public override void Die()
	{
		gameObject.GetComponent<SpriteRenderer>().flipY = true;
		Destroy(gameObject, 1f);
		base.Die();
	}

	private void OnTakeDamage() // when pressing SPACE
	{
        if (isAlive)
        {
            if (GetComponent<PhotonView>().IsMine)
            {
				Debug.Log("Space-Damage");
				//gameObject.GetComponent<PlayerStats>().TakeDamage(5);
				gameObject.GetComponent<PlayerStats>().GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, 5f);
			}
		}
	}

	//void Start()
	//{
	//    EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
	//}

	void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
	{
		if (newItem != null)
		{
			maxHealth.AddModifierAdd(newItem.healthModifierAdd);
			armor.AddModifierAdd(newItem.armorModifierAdd);
			damage.AddModifierAdd(newItem.damageModifierAdd);
			evade.AddModifierAdd(newItem.evadeModifierAdd);

			maxHealth.AddModifierMultiply(newItem.healthModifierMultiply);
			armor.AddModifierMultiply(newItem.armorModifierMultiply);
			damage.AddModifierMultiply(newItem.damageModifierMultiply);
			evade.AddModifierMultiply(newItem.evadeModifierMultiply);
		}

		if (oldItem != null)
		{
			maxHealth.RemoveModifierAdd(oldItem.healthModifierAdd);
			armor.RemoveModifierAdd(oldItem.armorModifierAdd);
			damage.RemoveModifierAdd(oldItem.armorModifierAdd);
			evade.RemoveModifierAdd(oldItem.evadeModifierAdd);

			maxHealth.RemoveModifierMultiply(oldItem.healthModifierMultiply);
			armor.RemoveModifierMultiply(oldItem.armorModifierMultiply);
			damage.RemoveModifierMultiply(oldItem.armorModifierMultiply);
			evade.RemoveModifierMultiply(oldItem.evadeModifierMultiply);
		}
	}
}
