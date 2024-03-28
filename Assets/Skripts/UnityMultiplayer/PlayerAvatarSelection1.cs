using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarSelection1 : MonoBehaviour
{
    [SerializeField] private int playerIndex;

    private void Start()
    {
        MultiplayerGroupManager.MyInstance.OnMultiplayerPlayerDatasChanged += MultiplayerGroupManager_OnMultiplayerPlayerDatasChanged;
        UpdatePlayer();
    }


    private void MultiplayerGroupManager_OnMultiplayerPlayerDatasChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer(); // Immer wenn die Liste der PlayerDaten (ClientIds usw.) sich ändert, soll jeder Avatar aktualisiert werden
    }

    private void UpdatePlayer()
    {
        Debug.Log("UpdatePlayer wird ausgeführt");
        if (MultiplayerGroupManager.MyInstance.IsPlayerIndexConnected(playerIndex))
        { Show(); }
        else
        { Hide(); }
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
