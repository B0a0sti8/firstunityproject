using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class PlayerTargetInfoUI : MonoBehaviour
{
    [SerializeField] HealthBar myHealthBar;
    [SerializeField] ManaBar myManaBar;
    [SerializeField] GameObject myTargetName;
    [SerializeField] GameObject myTargetLevel;
    GameObject myTemporaryTarget;

    void Start()
    {
        //myHealthBar = transform.Find("HealthBar").GetComponent<HealthBar>();
        //myManaBar = transform.Find("ManaBar").GetComponent<ManaBar>();
        //myTargetName = transform.Find("TargetName").Find("Text (TMP)").gameObject;
        //myTargetLevel = transform.Find("TargetLevel").Find("Text (TMP)").gameObject;

    }


    public void UpdateMyUI()
    {
        InteractionCharacter myPlayerObjectInteract = transform.parent.parent.parent.GetComponent<InteractionCharacter>();

        if (myPlayerObjectInteract.focus == null)
        {
            Hide();
            return;
        }

        Show();

        Transform myTarget = myPlayerObjectInteract.focus.transform;
        myTemporaryTarget = myPlayerObjectInteract.focus.gameObject;

        if (myTarget.tag == "Player")
        {
            // Hier werden alle Parameter im Fenster initail gesetzt, falls es ein Player ist.
            myTargetName.GetComponent<TextMeshProUGUI>().text = myTarget.GetComponent<StuffManagerScript>().GetCharacterName().ToString();

            myHealthBar.SetMaxHealth((int)myTarget.GetComponent<PlayerStats>().maxHealthServer.Value);
            myHealthBar.SetHealth((int)myTarget.GetComponent<PlayerStats>().currentHealth.Value);

            myManaBar.SetMaxMana((int)myTarget.GetComponent<PlayerStats>().maxManaServer.Value);
            myManaBar.SetMana((int)myTarget.GetComponent<PlayerStats>().currentMana.Value);

            myTargetLevel.GetComponent<TextMeshProUGUI>().text = myTarget.GetComponent<PlayerStats>().MyCurrentPlayerLvl.ToString();

            // Um Änderungen in Leben und Mana zu erfassen subscribed man auf Änderungen der Networkvariablen. Das sollte man rückgängig machen, wenn das Ziel gewechselt / entfernt wird.
            myTemporaryTarget.GetComponent<CharacterStats>().currentHealth.OnValueChanged += SetHealthValue;
            myTemporaryTarget.GetComponent<CharacterStats>().maxHealthServer.OnValueChanged += SetMaxHealthValue;

            myTemporaryTarget.GetComponent<PlayerStats>().currentMana.OnValueChanged += SetManaValue;
            myTemporaryTarget.GetComponent<PlayerStats>().maxManaServer.OnValueChanged += SetMaxManaValue;

        }
        else if (myTarget.tag == "Ally" || myTarget.tag == "Enemy")
        {
            // Hier werden alle Parameter im Fenster initail gesetzt, falls es ein Gegner oder Ally ist.
            // Hier müssen nur Leben, Level und Name gemacht werden.

            myTargetName.GetComponent<TextMeshProUGUI>().text = myTarget.GetComponent<EnemyAI>().enemyName;

            myHealthBar.SetMaxHealth((int)myTarget.GetComponent<EnemyStats>().maxHealthServer.Value);
            myHealthBar.SetHealth((int)myTarget.GetComponent<EnemyStats>().currentHealth.Value);

            myTargetLevel.GetComponent<TextMeshProUGUI>().text = myTarget.GetComponent<EnemyAI>().enemyLevel.ToString();

            // Um Änderungen in Leben und Mana zu erfassen subscribed man auf Änderungen der Networkvariablen. Das sollte man rückgängig machen, wenn das Ziel gewechselt / entfernt wird.
            myTemporaryTarget.GetComponent<CharacterStats>().currentHealth.OnValueChanged += SetHealthValue;
            myTemporaryTarget.GetComponent<CharacterStats>().maxHealthServer.OnValueChanged += SetMaxHealthValue;
        }
    }


    private void SetHealthValue(float prev, float newValue)
    {
        myHealthBar.SetHealth((int)newValue);
    }

    private void SetMaxHealthValue(float prev, float newValue)
    {
        myHealthBar.SetMaxHealth((int)newValue);
    }

    private void SetManaValue(float prev, float newValue)
    {
        myManaBar.SetMana((int)newValue);
    }

    private void SetMaxManaValue(float prev, float newValue)
    {
        myManaBar.SetMaxMana((int)newValue);
    }

    public void OnTargetLost()
    {
        if (myTemporaryTarget == null)
        {
            return;
        }
        if (myTemporaryTarget.tag == "Player")
        {
            Debug.Log("Unsubscribing from events!");
            myTemporaryTarget.GetComponent<CharacterStats>().currentHealth.OnValueChanged -= SetHealthValue;
            myTemporaryTarget.GetComponent<CharacterStats>().maxHealthServer.OnValueChanged -= SetMaxHealthValue;

            myTemporaryTarget.GetComponent<PlayerStats>().currentMana.OnValueChanged -= SetManaValue;
            myTemporaryTarget.GetComponent<PlayerStats>().maxManaServer.OnValueChanged -= SetMaxManaValue;
        }
        else if (myTemporaryTarget.tag == "Enemy" || myTemporaryTarget.tag == "Ally")
        {
            myTemporaryTarget.GetComponent<CharacterStats>().currentHealth.OnValueChanged -= SetHealthValue;
            myTemporaryTarget.GetComponent<CharacterStats>().maxHealthServer.OnValueChanged -= SetMaxHealthValue;
        }

        myTemporaryTarget = null;
        Hide();
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
