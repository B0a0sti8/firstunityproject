using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class HostKickedYouUI : MonoBehaviour
{
    [SerializeField] Button returnMainMenuButton;

    // Start is called before the first frame update
    private void Awake()
    {
        returnMainMenuButton.onClick.AddListener(() => { ReturnToMainMenu(); });
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong obj)
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void ReturnToMainMenu()
    {
        NetworkManager.Singleton.Shutdown();
        GameObject netWo = GameObject.Find("NetworkManager");
        Destroy(netWo);
        SceneManager.LoadScene("MainMenu");
    }
}
