using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour {


    [SerializeField] private Button authenticateButton;
    [SerializeField] private GameObject lobbyList;


    private void Awake() {
        authenticateButton.onClick.AddListener(() => {
            ApplyAuthenticate();
        });
    }

    private void ApplyAuthenticate()
    {
        string playerName = EditPlayerName.Instance.GetPlayerName();
        LobbyManager.Instance.Authenticate(playerName);
        Hide();
        lobbyList.SetActive(true);
        lobbyList.transform.Find("YourName").GetComponent<TextMeshProUGUI>().text = "Your Name: " + playerName;

    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}