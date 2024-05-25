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
    private Transform currentCharacterButton;
    private Transform enterCharacterNameWindow;
    private Button createNewCharacterButton;
    private Button createNewCharacterOk;
    private Button createNewCharacterCancel;
    private GameObject characterAlreadyExistsWindow;


    private void Awake()
    {
        FetchAllCharacters();
        applyButton = transform.Find("ApplyButton").GetComponent<Button>();
        applyButton.onClick.AddListener(() => { ApplyButtonClick(); });

        currentCharacterButton = transform.Find("CurrentCharacter");
        enterCharacterNameWindow = transform.Find("EnterCharacterNameWindow");

        enterCharacterNameWindow.gameObject.SetActive(false);

        createNewCharacterButton = transform.Find("CreateNewCharacterButton").GetComponent<Button>(); 
        createNewCharacterButton.onClick.AddListener(() => { CreateNewCharacterButtonClick(); });

        createNewCharacterOk = enterCharacterNameWindow.Find("Image").Find("OKButton").GetComponent<Button>();
        createNewCharacterOk.onClick.AddListener(() => { CreateNewCharacterOk(); });

        createNewCharacterCancel = enterCharacterNameWindow.Find("Image").Find("CancelButton").GetComponent<Button>();
        createNewCharacterCancel.onClick.AddListener(() => { CreateNewCharacterCancel(); });

        characterAlreadyExistsWindow = transform.Find("CharacterAlreadyExistsWindow").gameObject;
        characterAlreadyExistsWindow.SetActive(false);
        characterAlreadyExistsWindow.transform.Find("Image").Find("OKButton").GetComponent<Button>().onClick.AddListener(() => CloseCharacterAlreadyExistsWindow());

    }

    private void ApplyButtonClick()
    {
        string[] convTex = currentCharacterButton.Find("Image1").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text.Split("\n");
        characterName = convTex[0];

        if (characterName != "" && characterName != "Empty")
        {
            playerReadyButton.interactable = true;

            MultiplayerGroupManager.MyInstance.SetNewNetworkListString(characterName);
            Hide();
        }
    }

    private void CreateNewCharacterButtonClick()
    {
        enterCharacterNameWindow.gameObject.SetActive(true);
    }

    private void CreateNewCharacterOk()
    {
        
        characterAlreadyExistsWindow.SetActive(false);

        string newCharName = enterCharacterNameWindow.Find("Image").Find("InputField (TMP)").GetComponent<TMP_InputField>().text;
        Debug.Log("Neuer Charaktername: " + newCharName);
        bool doesCharacterAlreadyExist = CheckIfCharacterNameExistsAlready(newCharName);

        if (doesCharacterAlreadyExist)
        {
            characterAlreadyExistsWindow.SetActive(true);
            return;
        }

        currentCharacterButton.Find("Image1").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = newCharName + "\n Level: 1";
        currentCharacterButton.Find("Image2").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Quest: " + "No quest yet";

        enterCharacterNameWindow.gameObject.SetActive(false);
    }

    private void CreateNewCharacterCancel()
    {
        enterCharacterNameWindow.gameObject.SetActive(false);
    }

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

    public void LoadCharacterOnButtonPress(Transform button)
    {
        loadButtonParent = transform.Find("LoadCharacter").Find("ExistingCharactersParent");

        foreach(Transform child in loadButtonParent)
        {
            if (child == button)
            {
                string string1 = button.Find("Image1").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
                string string2 = button.Find("Image2").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;

                currentCharacterButton.Find("Image1").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = string1;
                currentCharacterButton.Find("Image2").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = string2;
            }
        }
    }


    private bool CheckIfCharacterNameExistsAlready(string newCharacterName)
    {
        bool characterExists = false;

        // Sammle alle gespeicherten Charactere und hole dir den Namen, vergleiche alle mit dem neuen Namen. Wenn der neue name schon existiert, wird die Funktion false zurück geben. Ansonsten True
        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles();
        foreach (FileInfo fInfo in fileInfo)
        {
            string[] ident = fInfo.Name.Split("_");
            if (ident[0] == "SaveFile")
            {
                string characterName = ident[1];
                if (characterName == newCharacterName) { characterExists = true; break; }
            }
        }
        return characterExists;
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

    private void CloseCharacterAlreadyExistsWindow()
    {
        characterAlreadyExistsWindow.SetActive(false);
    }
}
