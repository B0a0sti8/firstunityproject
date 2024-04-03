using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using Unity.Collections;

public class PlayerAvatarSelection1 : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyTextObject;
    [SerializeField] private GameObject playerCharacterNameObject;

    [SerializeField] private Button kickPlayerButton;

    [SerializeField] private SkinnedMeshRenderer characterMeshRenderer;
    [SerializeField] private Material startMaterial;
    private Material material;

    private void Awake()
    {
        kickPlayerButton.onClick.AddListener(() => {
            MultiplayerPlayerData playerData = MultiplayerGroupManager.MyInstance.GetPlayerDataFromPlayerIndex(playerIndex);
            MultiplayerGroupManager.MyInstance.KickPlayer(playerData.clientId);
        });
    }

    private void Start()
    {
        MultiplayerGroupManager.MyInstance.OnMultiplayerPlayerDatasChanged += MultiplayerGroupManager_OnMultiplayerPlayerDatasChanged;      // Immer wenn sich im Multiplayer Manager ein Event ändert, führt er hier diese Funktion aus.
        CharacterSelectionManager.Instance.OnReadyChanged += CharacterSelectionManager_OnReadyChanged;

        kickPlayerButton.gameObject.SetActive(NetworkManager.Singleton.IsHost);

        material = new Material(startMaterial);
        characterMeshRenderer.material = material;

        UpdatePlayer();
    }

    private void CharacterSelectionManager_OnReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void MultiplayerGroupManager_OnMultiplayerPlayerDatasChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer(); // Immer wenn die Liste der PlayerDaten (ClientIds usw.) sich ändert, soll jeder Avatar aktualisiert werden
    }

    // Zeigt oder versteckt diesen Spieler Charakter. Ändert Farbe und ggf. Spielerdaten
    private void UpdatePlayer()
    {
        Debug.Log("UpdatePlayer wird ausgeführt");
        if (MultiplayerGroupManager.MyInstance.IsPlayerIndexConnected(playerIndex))
        { 
            Show();
            MultiplayerPlayerData playerData = MultiplayerGroupManager.MyInstance.GetPlayerDataFromPlayerIndex(playerIndex);
            readyTextObject.SetActive(CharacterSelectionManager.Instance.IsPlayerReady(playerData.clientId));
            SetPlayerColor(MultiplayerGroupManager.MyInstance.GetPlayerColor(playerData.colorId));

            string plName = playerData.playerName.ToString();
            string charName = playerData.characterName.ToString();

            if (playerData.characterName.ToString() == "")
            {
                charName = "has not chosen any character yet. ";
            }
            else
            {
                charName = "as " + charName;
            }

            playerCharacterNameObject.GetComponent<TextMeshProUGUI>().text = plName + "\n" + charName;

            kickPlayerButton.gameObject.SetActive(NetworkManager.Singleton.IsHost);
        }
        else
        { Hide(); }
    }

    public void SetPlayerColor(Color color)
    {
        material.color = color;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }


}
