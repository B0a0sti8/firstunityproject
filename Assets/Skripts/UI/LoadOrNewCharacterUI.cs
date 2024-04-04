using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadOrNewCharacterUI : MonoBehaviour
{
    private Button applyButton;
    [SerializeField] private Button playerReadyButton;
    private string characterName;
    private Transform loadButtonParent;
    private GameObject currentCharacterButton;

    private void Awake()
    {
        FetchAllCharacters();
        applyButton = transform.Find("ApplyButton").GetComponent<Button>();
        applyButton.onClick.AddListener(() => { ApplyButtonClick(); });
    }

    private void ApplyButtonClick()
    {
        characterName = "";
        const string glyphs = "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ";

        int charAmount = UnityEngine.Random.Range(7, 13);
        for (int i = 0; i < charAmount; i++)
        {
            characterName += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
        }

        Hide(); 
        playerReadyButton.interactable = true;

        MultiplayerGroupManager.MyInstance.SetNewNetworkListString(characterName);
    }

    [Command]
    private void FetchAllCharacters()
    {
        // Hier wird der Neu-Erstell-Button aktiviert. Characterslots geladen usw.
        loadButtonParent = transform.Find("LoadCharacter").Find("ExistingCharactersParent");
        Button createNewButton = transform.Find("CreateNewCharacterButton").GetComponent<Button>();
        createNewButton.interactable = true;
        GameObject cannotCreateNew = transform.Find("CannotCreateNew").gameObject;
        cannotCreateNew.SetActive(false);

        

        // Sammle alle gespeicherten Charactere
        // Trage Name, Level und derzeitige Hauptquest in den jeweiligen Button ein.
        // Application.persistentDataPath
        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles();
        int en = 0;
        foreach (FileInfo fInfo in fileInfo)
        {
            if (en > 7)
            {
                // Die Speicherplätze sind alle voll. Deaktiviere den Neu-Erstellen-Button und füge einen kleinen Warnungstext ein.
                cannotCreateNew.SetActive(true);
                createNewButton.interactable = false;
                break; 
            }

            string[] ident = fInfo.Name.Split("_");
            if (ident[0]=="SaveFile")
            {
                Debug.Log(fInfo.Name);
                string characterName = ident[1];
                string characterLevel = ident[2];
                string characterMainQuest = ident[3];

                Debug.Log(loadButtonParent);
                Debug.Log("ExistingCharacter_" + en);
                Transform loadButton = loadButtonParent.Find("ExistingCharacter_" + en);
                loadButton.Find("Image1").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = characterName + "\n" + "Level: " + characterLevel;
                loadButton.Find("Image2").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Quest: " + characterMainQuest;

                en += 1;
            }
        }
    }


    public void Show()
    {
        FetchAllCharacters();
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
