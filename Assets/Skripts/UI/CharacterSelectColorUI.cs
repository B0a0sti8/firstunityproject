using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectColorUI : MonoBehaviour
{
    [SerializeField] private int colorId;
    [SerializeField] private Image image;
    [SerializeField] private GameObject selectedGameObject;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => { MultiplayerGroupManager.MyInstance.ChangePlayerColor(colorId); });
    }

    private void Start()
    {
        MultiplayerGroupManager.MyInstance.OnMultiplayerPlayerDatasChanged += MultiplayerGroupManager_OnMultiplayerPlayerDatasChanged;
        image.color = MultiplayerGroupManager.MyInstance.GetPlayerColor(colorId);
    }

    private void MultiplayerGroupManager_OnMultiplayerPlayerDatasChanged(object sender, System.EventArgs e)
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        if(MultiplayerGroupManager.MyInstance.GetPlayerData().colorId == colorId && selectedGameObject != null)
        {
            selectedGameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        MultiplayerGroupManager.MyInstance.OnMultiplayerPlayerDatasChanged -= MultiplayerGroupManager_OnMultiplayerPlayerDatasChanged;
    }
}
