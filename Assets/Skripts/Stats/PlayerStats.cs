using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

// tasten-input wird bei BEIDEN AUSGELÖST
// button-input wird bei dem ausgelöst, dessen Button über dem anderen liegt!
// Beides gelöst durch, generelles deaktivieren von "Canvas Action Skills"

// master-skripts werden noch bei beiden ausgeführt

// öffentliche variablen benötigt (z.B speed, mana, health)

public class PlayerStats : CharacterStats, IPunObservable
{
	//public PhotonView view;

	[Header("Mana")]
	public float maxMana = 1000;
	public float currentMana;
	[HideInInspector]
	public ManaBar manaBar;
	[HideInInspector]
	public ManaBar manaBarUI;

	TextMeshProUGUI healthText;
	TextMeshProUGUI manaText;

	float tickEveryXSeconds = 1f; // gain mana every 1 second
	float tickEveryXSecondsTimer = 0f;

	public int missRandom;



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
			gameObject.transform.Find("Own Canvases").gameObject.SetActive(true);
		}

		currentHealth = maxHealth.GetValue();
		currentMana = maxMana;
		manaBar = transform.Find("Canvases").transform.Find("Canvas World Space").transform.Find("ManaBar").GetComponent<ManaBar>();
		manaBarUI = transform.Find("Own Canvases").transform.Find("Canvas Healthbar UI").transform.Find("ManaBar").GetComponent<ManaBar>();

		healthText = transform.Find("Own Canvases").transform.Find("Canvas Healthbar UI").transform.Find("HealthBar").transform.Find("Health Text").GetComponent<TextMeshProUGUI>();
		manaText = transform.Find("Own Canvases").transform.Find("Canvas Healthbar UI").transform.Find("ManaBar").transform.Find("Mana Text").GetComponent<TextMeshProUGUI>();
		
		isAlive = true;

		EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
	}

	private void Update()
	{
		// updates Bars on Canvases
		transform.Find("Canvases").transform.Find("Canvas World Space").transform.Find("HealthBar").GetComponent<HealthBar>().SetMaxHealth((int)maxHealth.GetValue());
		transform.Find("Canvases").transform.Find("Canvas World Space").transform.Find("HealthBar").GetComponent<HealthBar>().SetHealth((int)currentHealth);
		transform.Find("Own Canvases").transform.Find("Canvas Healthbar UI").transform.Find("HealthBar").GetComponent<HealthBar>().SetMaxHealth((int)maxHealth.GetValue());
		transform.Find("Own Canvases").transform.Find("Canvas Healthbar UI").transform.Find("HealthBar").GetComponent<HealthBar>().SetHealth((int)currentHealth);
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
		manaBarUI.SetMaxMana((int)maxMana);
		manaBarUI.SetMana((int)currentMana);
		if (currentMana > maxMana)
		{
			currentMana = maxMana;
		}
		if (currentMana < 0)
		{
			currentMana = 0;
		}

		healthText.SetText(currentHealth.ToString().Replace(",", ".") + " / " + maxHealth.GetValue().ToString().Replace(",", "."));
		manaText.SetText(currentMana.ToString().Replace(",", ".") + " / " + maxMana.ToString().Replace(",", "."));

		// Mana regeneration
		tickEveryXSecondsTimer += Time.deltaTime;
		if (tickEveryXSecondsTimer >= tickEveryXSeconds)
		{
			tickEveryXSecondsTimer = 0f;

			ManageManaRPC(25f);
		}
	}

	[PunRPC]
	public override void TakeDamage(float damage, int missRandomRange)
	{
		base.TakeDamage(damage, missRandomRange);
	}

	public void TakeDamageRPC(float damage)
	{
		if (view.IsMine)
		{
			missRandom = Random.Range(0, 100);
			view.RPC("TakeDamage", RpcTarget.All, damage, missRandom);
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
		//TooltipScreenSpaceUI.ShowTooltip_Static("Hello");
		if (isAlive)
        {
			ManageManaRPC(-20f);

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