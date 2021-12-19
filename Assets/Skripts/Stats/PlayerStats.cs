using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// tasten-input wird bei BEIDEN AUSGELÖST
// button-input wird bei dem ausgelöst, dessen Button über dem anderen liegt!
// Beides gelöst durch, generelles deaktivieren von "Canvas Action Skills"

// master-skripts werden noch bei beiden ausgeführt

// öffentliche variablen benötigt (z.B speed, mana, health)

public class PlayerStats : CharacterStats, IPunObservable
{
	public PhotonView view;

	[Header("Mana")]
	public float maxMana = 1000;
	public float currentMana;
	[HideInInspector]
	public ManaBar manaBar;

	float tickEveryXSeconds = 1f; // gain mana every 1 second
	float tickEveryXSecondsTimer = 0f;



	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		// Reihenfolge der gesendeten und empfangenen Komponenten muss gleich sein
		if (stream.IsWriting)
		{
			stream.SendNext(currentHealth);
			stream.SendNext(currentMana);
		}
		else if (stream.IsReading)
		{
			currentHealth = (float)stream.ReceiveNext();
			currentMana = (float)stream.ReceiveNext();
		}
	}

	void Start()
	{
		if (view.IsMine)
		{
			gameObject.transform.Find("Canvas Action Skills").gameObject.SetActive(true);
		}

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

		manaBar.SetMaxMana((int)maxMana);
		manaBar.SetMana((int)currentMana);
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

			ManageManaRPC(25);
		}
	}

	[PunRPC]
	public override void TakeDamage(float damage)
	{
		base.TakeDamage(damage);
	}

	public void TakeDamageRPC(float damage)
	{
		if (view.IsMine)
		{
			view.RPC("TakeDamage", RpcTarget.All, damage);
		}
	}

	[PunRPC]
	private void ManageMana(float manaCost)
    {
		currentMana += manaCost;
	}

	public void ManageManaRPC(float manaCost)
	{
		if (view.IsMine)
		{
			view.RPC("ManageMana", RpcTarget.All, manaCost);
		}
	}

	public override void Die()
	{
		gameObject.GetComponent<SpriteRenderer>().flipY = true;
		//Destroy(gameObject, 1f);
		base.Die();
	}

	private void OnTakeDamage() // when pressing SPACE
	{
        if (isAlive)
        {
			ManageManaRPC(-100);

			TakeDamageRPC(20f);
		}
	}


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