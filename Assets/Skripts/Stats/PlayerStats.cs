using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

// tasten-input wird bei BEIDEN AUSGELÖST
// button-input wird bei dem ausgelöst, dessen Button über dem anderen liegt!
// Beides gelöst durch, generelles deaktivieren von "Canvas Action Skills"

// master-skripts werden noch bei beiden ausgeführt

// öffentliche variablen benötigt (z.B speed, mana, health)

// 

public class PlayerStats : CharacterStats, IPunObservable
{
	[Header("Class")] public string className;
	[Header("Gold")] public int goldAmount;

	#region Stats: Aus Characterstats geerbte sind auskommentiert
	[Header("Mana")]
	public Stat maxMana;
	public float currentMana;

	//[Header("Health")]
	//public Stat maxHealth;
	//public float currentHealth;				

	[Header("Main Stats")]
	// public Stat armor;					// Verringert erlittenen Schaden
	public Stat weaponDamage;
	public Stat mastery;				   	// Erhöht den Schaden (dmgInc) bzw. die Heilung (healInc) von Zaubern und Waffenangriffen
	public Stat toughness;				    // Erhöht Resistenz gg. magischen und physischen Schaden, Leben, erhaltene Heilung und Blockchance
	public Stat intellect;				    // Erhöht die Dauer bzw. den Radius von manchen Zaubern. Erhöht Crit-Chance und -Multiplikator
	public Stat charisma;                   // Erhöht den Effekt von Buffs und Debuffs
	public Stat tempo;                      // Erhöht Tick-Rate von HoTs bzw. DoTs. Erhöht actionSpeed und evade

	[Header("Side Stats")]
	//public Stat movementSpeed;			// Lässt Charakter schneller laufen
	//public Stat actionSpeed;				// Lässt Charakter schneller casten und angreifen. Niedrige Cooldowns, GCD
	//public Stat critChance;				// Chance auf 1.5-fachen Schaden
	//public Stat critMultiplier;			// Erhöht kritischen Schaden von 1.5 auf mehr.
	//public Stat evadeChance;				// Lässt Charakter Schaden vermeiden
	public Stat healInc;					// Erhöht verursachte Heilung
	public Stat dmgInc;						// Erhöht verursachten Schaden
	public Stat physRed;					// Verringert erlittenen physischen Schaden
	public Stat magRed;                     // Verringert erlittenen magischen Schaden
	public Stat incHealInc;					// Erhöht erhaltene Heilung
	public Stat blockChance;				// Erhöht Blockchance (nur bei Schildträgern und ggf. Dual wielding)
	public Stat skillRadInc;				// Höherer Skillradius
	public Stat skillDurInc;				// Höhere Skilldauer
	public Stat buffInc;					// Stärkerer Buffeffekt
	public Stat debuffInc;					// Stärkerer Debuffeffekt
	public Stat tickRateMod;				// Schnellere Tickrate für Effekte
	public Stat lifesteal;					// Heilt durch verursachten Schaden bzw. Heilung
	#endregion

	#region UI-Interface-Stuff
	ManaBar manaBar;
	ManaBar manaBarUI;

	TextMeshProUGUI healthText;
	TextMeshProUGUI manaText;

	public Sprite castingBarImage;
	public string castingBarText;
	public bool castingBarChanneling;
	#endregion

	private CharacterPanelScript charPanel;
	private CharPanelButtonScript[] allEquipSlots;

	public override void Update()
	{
		base.Update();

		SyncModifiedPlayerStats();

		UpdateHealthAndMana();

		ManaRegeneration();
	}

	void Start()
	{
		charPanel = transform.Find("Own Canvases").Find("CanvasCharacterPanel").Find("CharacterPanel").GetComponent<CharacterPanelScript>();
		allEquipSlots = charPanel.allEquipmentSlots;

		isAlive = true;

		ReloadEquipMainStats(); 

		currentHealth = maxHealth.GetValue();
		currentMana = maxMana.GetValue();

		if (view.IsMine)
		{
			gameObject.transform.Find("Own Canvases").gameObject.SetActive(true);
		}

		manaBar = transform.Find("Canvases").Find("Canvas World Space").Find("ManaBar").GetComponent<ManaBar>();
		manaBarUI = transform.Find("Own Canvases").Find("Canvas Healthbar UI").Find("ManaBar").GetComponent<ManaBar>();

		healthText = transform.Find("Own Canvases").Find("Canvas Healthbar UI").Find("HealthBar").Find("Health Text").GetComponent<TextMeshProUGUI>();
		manaText = transform.Find("Own Canvases").Find("Canvas Healthbar UI").Find("ManaBar").Find("Mana Text").GetComponent<TextMeshProUGUI>();
	}

	void InitializeBaseStats()
	{
		maxHealth.baseValue = 1000;		maxMana.baseValue = 1000;
		armor.baseValue = 10;			mastery.baseValue = 10;
		toughness.baseValue = 10;		intellect.baseValue = 10;
		charisma.baseValue = 10;		tempo.baseValue = 10;

		movementSpeed.baseValue = 7;	actionSpeed.baseValue = 1;
		critChance.baseValue = 5;		critMultiplier.baseValue = 150;
		healInc.baseValue = 0;			dmgInc.baseValue = 0;
		physRed.baseValue = 0;			magRed.baseValue = 0;
		incHealInc.baseValue = 0;		lifesteal.baseValue = 0;
		blockChance.baseValue = 0;		evadeChance.baseValue = 0;
		skillDurInc.baseValue = 0;		skillRadInc.baseValue = 0;
		buffInc.baseValue = 0;			debuffInc.baseValue = 0;
		tickRateMod.baseValue = 0;
	}

	public void UpdateMainStats(int modArmor = 0, int modWeaponDmg = 0, int modMastery = 0, int modToughness = 0, int modIntellect = 0, int modCharisma = 0, int modTempo = 0)
    {
		armor.baseValue += modArmor;
		weaponDamage.baseValue += modWeaponDmg;
		mastery.baseValue += modMastery;
		toughness.baseValue += modToughness;
		intellect.baseValue += modIntellect;
		charisma.baseValue += modCharisma;
		tempo.baseValue += modTempo;
	}

	public void ReloadEquipMainStats()
    {
		InitializeBaseStats();								// Setze alle Werte auf Standard

		for (int i = 0; i < allEquipSlots.Length; i++)		// Guck alle EquipmentSlots an
        {
			Equipment eq = allEquipSlots[i].MyEquip;

			if (eq != null)
            {
				UpdateMainStats(eq.armor, eq.weaponDamage, eq.mastery, eq.toughness, eq.intellect, eq.charisma, eq.tempo);
            }
			

		}
    }




	#region Multiplayer-Stuff
	void SyncModifiedPlayerStats()
	{
		maxHealth.modifiedValue = maxHealth.GetValue();
		armor.modifiedValue = armor.GetValue();
		movementSpeed.modifiedValue = movementSpeed.GetValue();
		actionSpeed.modifiedValue = actionSpeed.GetValue();
		critChance.modifiedValue = critChance.GetValue();
		critMultiplier.modifiedValue = critMultiplier.GetValue();

		evadeChance.modifiedValue = evadeChance.GetValue();
	}

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
	#endregion

	#region ManageManaAndHealth
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

		manaBar.SetMaxMana((int)maxMana.GetValue());
		manaBar.SetMana((int)currentMana);
		manaBarUI.SetMaxMana((int)maxMana.GetValue());
		manaBarUI.SetMana((int)currentMana);
		if (currentMana > maxMana.GetValue())
		{
			currentMana = maxMana.GetValue();
		}
		if (currentMana < 0)
		{
			currentMana = 0;
		}

		healthText.SetText(currentHealth.ToString().Replace(",", ".") + " / " + maxHealth.GetValue().ToString().Replace(",", "."));
		manaText.SetText(currentMana.ToString().Replace(",", ".") + " / " + maxMana.ToString().Replace(",", "."));
	}

	[PunRPC] private void ManageMana(float manaCost)
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

	void ManaRegeneration()
	{
		// gain mana every 1 second
		float tickEveryXSecondsTimer = 0f;
		float tickEveryXSeconds = 1f;
		tickEveryXSecondsTimer += Time.deltaTime;
		if (tickEveryXSecondsTimer >= tickEveryXSeconds)
		{
			tickEveryXSecondsTimer = 0f;

			ManageManaRPC(25f);
		}
	}

	[PunRPC] public override void GetHealing(float healing, int critRandomRange, float critChance, float critMultiplier)
	{
		base.GetHealing(healing, critRandomRange, critChance, critMultiplier);
	}

	[PunRPC] public override void TakeDamage(float damage, int missRandomRange, int critRandomRange, float critChance, float critMultiplier)
	{
		base.TakeDamage(damage, missRandomRange, critRandomRange, critChance, critMultiplier);
	}

	public void TakeDamageRPC(float damage)
	{
		if (view.IsMine)
		{
			int missRandom = Random.Range(1, 100);
			int critRandom = Random.Range(1, 100);
			float cChance = critChance.GetValue();
			float cMultiplier = critMultiplier.GetValue();
			view.RPC("TakeDamage", RpcTarget.All, damage, missRandom, critRandom, cChance, cMultiplier);
		}
	}

	public void TakeDamageSpace() // when pressing SPACE
	{
		//TooltipScreenSpaceUI.ShowTooltip_Static("Hello");
		if (isAlive)
		{
			ManageManaRPC(-20f);

			TakeDamageRPC(20f);
		}
	}
    #endregion

    public override void Die()
	{
		gameObject.GetComponent<SpriteRenderer>().flipY = true;
		//Destroy(gameObject, 1f);
		base.Die();
	}

	// VERALTET!!!!

 //   void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
	//{
	//	if (newItem != null)
	//	{
	//		maxHealth.AddModifierAdd(newItem.healthModifierAdd);
	//		armor.AddModifierAdd(newItem.armorModifierAdd);
	//		mastery.AddModifierAdd(newItem.damageModifierAdd);
	//		evade.AddModifierAdd(newItem.evadeModifierAdd);

	//		maxHealth.AddModifierMultiply(newItem.healthModifierMultiply);
	//		armor.AddModifierMultiply(newItem.armorModifierMultiply);
	//		mastery.AddModifierMultiply(newItem.damageModifierMultiply);
	//		evade.AddModifierMultiply(newItem.evadeModifierMultiply);
	//	}

	//	if (oldItem != null)
	//	{
	//		maxHealth.RemoveModifierAdd(oldItem.healthModifierAdd);
	//		armor.RemoveModifierAdd(oldItem.armorModifierAdd);
	//		mastery.RemoveModifierAdd(oldItem.armorModifierAdd);
	//		evade.RemoveModifierAdd(oldItem.evadeModifierAdd);

	//		maxHealth.RemoveModifierMultiply(oldItem.healthModifierMultiply);
	//		armor.RemoveModifierMultiply(oldItem.armorModifierMultiply);
	//		mastery.RemoveModifierMultiply(oldItem.armorModifierMultiply);
	//		evade.RemoveModifierMultiply(oldItem.evadeModifierMultiply);
	//	}
	//}
}