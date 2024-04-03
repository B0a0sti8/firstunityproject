using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToMainMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject canvasReturnToMainMenu;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => { canvasReturnToMainMenu.SetActive(true); });
    }

    void Start()
    {
        canvasReturnToMainMenu.SetActive(false);
    }
}
