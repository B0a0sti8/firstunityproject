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
	//[HideInInspector]
	ManaBar manaBar;
	//[HideInInspector]
	ManaBar manaBarUI;

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

	void SettingPlayerBaseStats()
    {
		maxHealth.baseValue = 500;
		mastery.baseValue = 0;
		armor.baseValue = 10;
		movementSpeed.baseValue = 7;
		attackSpeed.baseValue = 0;
		critChance.baseValue = 30;
		critMultiplier.baseValue = 200;
    }

	void Start()
	{
		SettingPlayerBaseStats();

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

	void SyncModifiedPlayerStats()
    {
		maxHealth.modifiedValue = maxHealth.GetValue();
		mastery.modifiedValue = mastery.GetValue();
		armor.modifiedValue = armor.GetValue();
		movementSpeed.modifiedValue = movementSpeed.GetValue();
		attackSpeed.modifiedValue = attackSpeed.GetValue();
		critChance.modifiedValue = critChance.GetValue();
		critMultiplier.modifiedValue = critMultiplier.GetValue();

		evade.modifiedValue = evade.GetValue();
	}

	void UpdateHealthAndMana()
    {
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
	}

	void ManaRegeneration()
    {
		tickEveryXSecondsTimer += Time.deltaTime;
		if (tickEveryXSecondsTimer >= tickEveryXSeconds)
		{
			tickEveryXSecondsTimer = 0f;

			ManageManaRPC(25f);
		}
	}

	public override void Update()
	{
		base.Update();

		SyncModifiedPlayerStats();

		UpdateHealthAndMana();

		ManaRegeneration();
	}

	[PunRPC]
	public override void TakeDamage(float damage, int missRandomRange, int critRandomRange, float critChance, float critMultiplier)
	{
		base.TakeDamage(damage, missRandomRange, critRandomRange, critChance, critMultiplier);
	}

	[PunRPC]
	public override void GetHealing(float healing, int critRandomRange, float critChance, float critMultiplier)
	{
		base.GetHealing(healing, critRandomRange, critChance, critMultiplier);
	}

	public void TakeDamageRPC(float damage)
	{
		if (view.IsMine)
		{
			missRandom = Random.Range(1, 100);
			int critRandom = Random.Range(1, 100);
			float cChance = critChance.GetValue();
			float cMultiplier = critMultiplier.GetValue();
			view.RPC("TakeDamage", RpcTarget.All, damage, missRandom, critRandom, cChance, cMultiplier);
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
			mastery.AddModifierAdd(newItem.damageModifierAdd);
			evade.AddModifierAdd(newItem.evadeModifierAdd);

			maxHealth.AddModifierMultiply(newItem.healthModifierMultiply);
			armor.AddModifierMultiply(newItem.armorModifierMultiply);
			mastery.AddModifierMultiply(newItem.damageModifierMultiply);
			evade.AddModifierMultiply(newItem.evadeModifierMultiply);
		}

		if (oldItem != null)
		{
			maxHealth.RemoveModifierAdd(oldItem.healthModifierAdd);
			armor.RemoveModifierAdd(oldItem.armorModifierAdd);
			mastery.RemoveModifierAdd(oldItem.armorModifierAdd);
			evade.RemoveModifierAdd(oldItem.evadeModifierAdd);

			maxHealth.RemoveModifierMultiply(oldItem.healthModifierMultiply);
			armor.RemoveModifierMultiply(oldItem.armorModifierMultiply);
			mastery.RemoveModifierMultiply(oldItem.armorModifierMultiply);
			evade.RemoveModifierMultiply(oldItem.evadeModifierMultiply);
		}
	}
}