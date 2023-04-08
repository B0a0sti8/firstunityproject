using TMPro;
using Unity.Netcode;
using UnityEngine;

// tasten-input wird bei BEIDEN AUSGELÖST
// button-input wird bei dem ausgelöst, dessen Button über dem anderen liegt!
// Beides gelöst durch, generelles deaktivieren von "Canvas Action Skills"

// master-skripts werden noch bei beiden ausgeführt

// öffentliche variablen benötigt (z.B speed, mana, health)

// 

public class PlayerStats : CharacterStats
{
	[Header("Class")] 
	public string mainClassName;
	public string rightSubClassName;
	public string leftSubClassName;

	[SerializeField] private bool isTank = false;
	[Header("Gold")] public int goldAmount;

	[Header("Experience")]
	[SerializeField] private int neededXP;
	[SerializeField] private int currentXP;
	[SerializeField] private int currentPlayerLvl;
	
	#region Stats: Aus Characterstats geerbte sind auskommentiert
	[Header("Mana")]
	public Stat maxMana;
	public NetworkVariable<float> currentMana = new NetworkVariable<float>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	//public float currentMana;

	//[Header("Health")]
	//public Stat maxHealth;
	//public float currentHealth;	
	private int maxHealthStart = 1000;		

	[Header("Main Stats")]
	// public Stat armor;					// Verringert erlittenen Schaden
	public Stat weaponDamage;
	public Stat mastery;				   	// Erhöht den Schaden (dmgInc) bzw. die Heilung (healInc) von Zaubern und Waffenangriffen
	public Stat toughness;				    // Erhöht Resistenz gg. magischen und physischen Schaden, Leben, erhaltene Heilung und Blockchance
	public Stat intellect;				    // Erhöht die Dauer bzw. den Radius von manchen Zaubern. Erhöht Crit-Chance und -Multiplikator sowie Mana.
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
	public Stat lifesteal;                  // Heilt durch verursachten Schaden bzw. Heilung

	public Stat[] allStatsArray;            // Enthält alle Stats
	#endregion

	#region MultiplayerStats

	#endregion

	float tickEveryXSecondsTimerMana = 0f;
	float tickEveryXSecondsMana = 1f;



	#region UI-Interface-Stuff
	ManaBar manaBarWorldCanv;
	ManaBar manaBarUI;

	TextMeshProUGUI healthText;
	TextMeshProUGUI manaText;

	XPBarScript xPBar;

	public Sprite castingBarImage;
	public string castingBarText;
	public bool castingBarChanneling;
	#endregion

	private CharacterPanelScript charPanel;
	private CharPanelButtonScript[] allEquipSlots;

	public bool MyIsTank { get => isTank; set => isTank = value; }

	public int MyNeededXP { get => neededXP; set => neededXP = value; }
    public int MyCurrentXP { get => currentXP; set => currentXP = value; }
    public int MyCurrentPlayerLvl { get => currentPlayerLvl; set => currentPlayerLvl = value; }

    public override void Update()
	{
		base.Update();

		if (!IsOwner) { return; }

		if (currentHealth.Value <= 0 && isAlive.Value == true)
		{
			Die();
			Debug.Log("Stirb!");
		}

		SyncModifiedPlayerStats();

		UpdateHealthAndMana();

		ManaRegeneration();

		//Debug.Log(gameObject.GetInstanceID());
	}

	public override void Start()
	{
		if (!IsOwner) { return; }

		manaBarWorldCanv = transform.Find("Canvas World Space").Find("ManaBar").GetComponent<ManaBar>();
		currentMana.OnValueChanged += (float previousValue, float newValue) => { OnManaChange(); };

		OnManaChange();

		gameObject.transform.Find("Own Canvases").gameObject.SetActive(true);

		xPBar = transform.Find("Own Canvases").Find("Canvas Healthbar UI").Find("XPBar").GetComponent<XPBarScript>();

		
		manaBarUI = transform.Find("Own Canvases").Find("Canvas Healthbar UI").Find("ManaBar").GetComponent<ManaBar>();

		healthText = transform.Find("Own Canvases").Find("Canvas Healthbar UI").Find("HealthBar").Find("Health Text").GetComponent<TextMeshProUGUI>();
		manaText = transform.Find("Own Canvases").Find("Canvas Healthbar UI").Find("ManaBar").Find("Mana Text").GetComponent<TextMeshProUGUI>();

		charPanel = transform.Find("Own Canvases").Find("CanvasCharacterPanel").Find("CharacterPanel").GetComponent<CharacterPanelScript>();
		allEquipSlots = charPanel.allEquipmentSlots;

		Stat[] allStatsArray = { armor, weaponDamage, mastery, toughness, intellect, charisma, tempo,
			movementSpeed, actionSpeed, critChance, critMultiplier, evadeChance, healInc, dmgInc, physRed,
			magRed, incHealInc, blockChance, skillRadInc, skillDurInc, buffInc, debuffInc, tickRateMod, lifesteal };

		ReloadEquipMainStats();

		//currentHealth.Value = maxHealth.GetValue();

		SetManaCurrentValueServerRPC(maxMana.GetValue());

		MyCurrentPlayerLvl = 1;
		MyNeededXP = Mathf.RoundToInt(100 * MyCurrentPlayerLvl * Mathf.Pow(MyCurrentPlayerLvl, 0.5f));
		MyCurrentXP = 10;
		xPBar.SetXPBar(MyCurrentXP, MyNeededXP);
		xPBar.UpdateLevel(MyCurrentPlayerLvl);

		goldAmount = 100;

		isAlive.Value = true;

		base.Start();

	}

    public override void TakeHealing(float healing, bool isCrit, NetworkBehaviourReference nBref)
    {
        base.TakeHealing(healing, isCrit, nBref);
    }

    public void OnManaChange()
	{
		if (!IsOwner) { return; }

		int cM = (int)this.currentMana.Value;
		int mM = (int)this.maxMana.GetValue();
		NetworkBehaviourReference nBref = this;
		ManaChangedServerRpc(cM, mM, nBref);
	}

	[ServerRpc]
	public void ManaChangedServerRpc(int cuMa, int maMa, NetworkBehaviourReference nBrf)
	{
		ManaChangedClientRpc(cuMa, maMa, nBrf);
	}

	[ClientRpc]
	public void ManaChangedClientRpc(int cuMa, int maMa, NetworkBehaviourReference nBrf)
	{
		nBrf.TryGet<CharacterStats>(out CharacterStats cStat);
		ManaBar maBa = cStat.transform.Find("Canvas World Space").Find("ManaBar").GetComponent<ManaBar>();
		maBa.SetMaxMana(maMa);
		maBa.SetMana(cuMa);
	}

	// Setzt alle Stats auf die Standardwerte eines nackten Charakters ohne Klasse, Talente und / oder Buffs und Debuffs
	void InitializeBaseStats()
	{
		maxHealth.baseValue = maxHealthStart;	maxMana.baseValue = 1000;
		armor.baseValue = 10;					mastery.baseValue = 10;
		toughness.baseValue = 10;				intellect.baseValue = 10;
		charisma.baseValue = 10;				tempo.baseValue = 10;

		movementSpeed.baseValue = 7;			actionSpeed.baseValue = 1;
		critChance.baseValue = 5;				critMultiplier.baseValue = 1.5f;
		healInc.baseValue = 0;					dmgInc.baseValue = 0;
		physRed.baseValue = 0;					magRed.baseValue = 0;
		incHealInc.baseValue = 0;				lifesteal.baseValue = 0;
		blockChance.baseValue = 0;				evadeChance.baseValue = 0;
		skillDurInc.baseValue = 0;				skillRadInc.baseValue = 0;
		buffInc.baseValue = 0;					debuffInc.baseValue = 0;
		tickRateMod.baseValue = 0;
	}

	// Modifiziert den Base
	public void ModifyAllStats(int modArmor = 0, int modWeaponDmg = 0, int modMastery = 0, int modToughness = 0, int modIntellect = 0, int modCharisma = 0, int modTempo = 0,
		int modMovement = 0, int modActionSpeed = 0, int modCritChance = 0, int modCritMult = 0, int modEvadeChance = 0, int modHealInc = 0, int modDmgInc = 0,
		int modPhysRed = 0, int modMagRed = 0, int modIncHealInc = 0, int modBlockChance = 0, int modSkillRad = 0, int modSkillDur = 0, int modBuffInc = 0, int modDebuffInc = 0,
		int modTickRate = 0, int modLifeSteal = 0)
    {
		armor.baseValue += modArmor; weaponDamage.baseValue += modWeaponDmg;
		mastery.baseValue += modMastery; toughness.baseValue += modToughness;
		intellect.baseValue += modIntellect; charisma.baseValue += modCharisma;
		tempo.baseValue += modTempo;

		movementSpeed.baseValue += modMovement; actionSpeed.baseValue += modActionSpeed;
		critChance.baseValue += modCritChance; critMultiplier.baseValue += modCritMult;
		evadeChance.baseValue += modEvadeChance; blockChance.baseValue += modBlockChance;
		healInc.baseValue += modHealInc; dmgInc.baseValue += modDmgInc;
		physRed.baseValue += modPhysRed; magRed.baseValue += modMagRed;
		incHealInc.baseValue += modIncHealInc; lifesteal.baseValue += modLifeSteal;
		skillRadInc.baseValue += modSkillRad; skillDurInc.baseValue += modSkillDur;
		buffInc.baseValue += modBuffInc; debuffInc.baseValue += modDebuffInc;
		tickRateMod.baseValue += modTickRate;
	}

	public void ReloadEquipMainStats()
    {
		InitializeBaseStats();								// Setze alle Werte auf Standard

		for (int i = 0; i < allEquipSlots.Length; i++)		// Guck alle EquipmentSlots an
        {
			Equipment eq = allEquipSlots[i].MyEquip;

			if (eq != null)
            {
				ModifyAllStats(eq.armor, eq.weaponDamage, eq.mastery, eq.toughness, eq.intellect, eq.charisma, eq.tempo,
					eq.movementSpeed, eq.actionSpeed, eq.critChance, eq.critMultiplier, eq.evadeChance, eq.healInc, eq.dmgInc,
					eq.physRed, eq.magRed, eq.incHealInc, eq.blockChance, eq.skillRadInc, eq.skillDurInc, eq.buffInc, eq.debuffInc,
					eq.tickRateMod, eq.lifesteal);
            }
		}
		ComputeSideStats();

	}

	private void ComputeSideStats()
    {
		// Mastery Based
		healInc.baseValue += 0.01f * mastery.GetValue();
		dmgInc.baseValue += 0.01f * mastery.GetValue();

		// Toughness Based
		physRed.baseValue += 0.01f * toughness.GetValue();
		magRed.baseValue += 0.01f * toughness.GetValue();
		incHealInc.baseValue += 0.01f * toughness.GetValue();
		blockChance.baseValue += 0.01f * toughness.GetValue();
		maxHealth.baseValue += 10f * toughness.GetValue();

		// Intellect Based
		skillDurInc.baseValue += 0.01f * intellect.GetValue();
		skillRadInc.baseValue += 0.01f * intellect.GetValue();
		critChance.baseValue += 0.01f * intellect.GetValue();
		critMultiplier.baseValue += 0.01f * intellect.GetValue();
		maxMana.baseValue += 10f * intellect.GetValue();

		// Charisma
		buffInc.baseValue += 0.01f * charisma.GetValue();
		debuffInc.baseValue += 0.01f * charisma.GetValue();

		// Tempo
		tickRateMod.baseValue += 0.01f * tempo.GetValue();
		evadeChance.baseValue += 0.01f * tempo.GetValue();
		actionSpeed.baseValue += 0.01f * tempo.GetValue();
    }

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

	#region ManageManaAndHealth
	void UpdateHealthAndMana()
	{
		//Wird ab jetzt in Characterstats gemacht.
		//transform.Find("Canvases").transform.Find("Canvas World Space").transform.Find("HealthBar").GetComponent<HealthBar>().SetMaxHealth((int)maxHealth.GetValue());
		//transform.Find("Canvases").transform.Find("Canvas World Space").transform.Find("HealthBar").GetComponent<HealthBar>().SetHealth((int)(currentHealth.Value));

		transform.Find("Own Canvases").transform.Find("Canvas Healthbar UI").transform.Find("HealthBar").GetComponent<HealthBar>().SetMaxHealth((int)maxHealth.GetValue());
		transform.Find("Own Canvases").transform.Find("Canvas Healthbar UI").transform.Find("HealthBar").GetComponent<HealthBar>().SetHealth((int)(currentHealth.Value));
		if (currentHealth.Value > maxHealth.GetValue())
		{
			SetCurrentHealthServerRpc(maxHealth.GetValue());
		}
		if (currentHealth.Value < 0)
		{
			SetCurrentHealthServerRpc(0);
		}

		manaBarUI.SetMaxMana((int)maxMana.GetValue());
		manaBarUI.SetMana((int)currentMana.Value);
		if (currentMana.Value > maxMana.GetValue())
		{
			SetManaCurrentValueServerRPC(maxMana.GetValue());
		}
		if (currentMana.Value < 0)
		{
			SetManaCurrentValueServerRPC(0);
		}

		string hText = currentHealth.Value.ToString().Replace(",", ".") + " / " + maxHealth.GetValue().ToString().Replace(",", ".");
		string mText = currentMana.Value.ToString().Replace(",", ".") + " / " + maxMana.GetValue().ToString().Replace(",", ".");

		healthText.SetText(hText);
		manaText.SetText(mText);
	}


	[ServerRpc]
	private void SetManaCurrentValueServerRPC(float manaValue)
	{
		currentMana.Value = manaValue;
	}


	[ServerRpc]
	private void ManageManaServerRPC(float manaCost)
	{
		currentMana.Value += manaCost;
	}

	//[ServerRpc]
	//public override void GetHealingServerRpc(float healing, bool isCrit)
	//{
	//	base.GetHealingServerRpc(healing, isCrit);
	//}

	//[ServerRpc]
	//public override void TakeDamageServerRpc(float damage, int aggro, bool isCrit)
	//{
	//	base.TakeDamageServerRpc(damage, aggro, isCrit);
	//}

	//public void TakeDamage(float damage, int aggro, bool isCrit, GameObject source)
	//{
	//	if (IsOwner)
	//	{
	//		TakeDamageServerRpc(damage, aggro, isCrit);
	//	}
	//}

	//public void TakeHealing(float healing, bool isCrit, GameObject source)
	//{
	//	if (IsOwner)
	//	{
	//		GetHealingServerRpc(healing, isCrit);
	//	}
	//}

	public void ManageMana(float manaCost)
	{
		ManageManaServerRPC(manaCost);
	}

	void ManaRegeneration()
	{
		// gain mana every 1 second
		tickEveryXSecondsTimerMana += Time.deltaTime;
		if (tickEveryXSecondsTimerMana >= tickEveryXSecondsMana)
		{
			tickEveryXSecondsTimerMana = 0f;

			ManageMana(25f);
		}
	}

	#endregion

	public void GainXP(int xp)
    {
		currentXP += xp;
		xPBar.SetXPBar(currentXP, MyNeededXP);
		if (currentXP >= neededXP)
        {
			int tooMuchXP = currentXP - neededXP;
			LevelUp(tooMuchXP);
        }
    }

	public void LevelUp(int tooMuchXP)
    {
		MyCurrentPlayerLvl++;
		xPBar.UpdateLevel(MyCurrentPlayerLvl);
		MyNeededXP = Mathf.RoundToInt(100 * MyCurrentPlayerLvl * Mathf.Pow(MyCurrentPlayerLvl, 0.5f));
		xPBar.SetXPBar(tooMuchXP, MyNeededXP);
		currentXP = tooMuchXP;

		if (currentXP >= neededXP)
		{
			tooMuchXP = currentXP - neededXP;
			LevelUp(tooMuchXP);
		}
	}

	public void LoadPlayerLevel()
    {
		xPBar.UpdateLevel(MyCurrentPlayerLvl);
		MyNeededXP = Mathf.RoundToInt(100 * MyCurrentPlayerLvl * Mathf.Pow(MyCurrentPlayerLvl, 0.5f));
		xPBar.SetXPBar(currentXP, MyNeededXP);
	}

    public override void Die()
	{
		gameObject.GetComponent<SpriteRenderer>().flipY = true;
		//Destroy(gameObject, 1f);
		base.Die();
	}



	// Ab hier nur zum Debuggen

	public void TakeDamageSpace() // when pressing SPACE
	{
		if (!IsOwner) { return; }

		if (isAlive.Value)
		{
			//ManageMana(-20f);

			//TakeDamage(20f, 0, false, gameObject);

			TakeDamage(20, 0, false, this);

			ManageMana(-20);
		}
	}

	public void CheatCodeAddXP()
	{
		if (!IsOwner) { return; }
		GainXP(505);
	}
}