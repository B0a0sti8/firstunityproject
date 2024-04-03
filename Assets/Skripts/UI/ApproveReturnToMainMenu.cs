using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class ApproveReturnToMainMenu : MonoBehaviour
{
    private Button noButton;
    private Button yesButton;
    private GameObject canvasReturn;

    private void Awake()
    {
        noButton = transform.Find("Image").Find("NoButton").GetComponent<Button>();
        yesButton = transform.Find("Image").Find("YesButton").GetComponent<Button>();
        canvasReturn = transform.parent.gameObject;

        noButton.onClick.AddListener(() => { canvasReturn.SetActive(false); });
        yesButton.onClick.AddListener(() => { ReturnToMainMenu(); });
    }

    private void ReturnToMainMenu()
    {
        NetworkManager.Singleton.Shutdown();
        GameObject netWo = GameObject.Find("NetworkManager");
        Destroy(netWo);
        SceneManager.LoadScene("MainMenu");
    }
}
