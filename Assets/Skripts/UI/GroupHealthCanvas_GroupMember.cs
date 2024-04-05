using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class GroupHealthCanvas_GroupMember : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouseOver = false;
    public GameObject myPlayerObject;
    public string myPlayerName = "defaultPlayerName";
    private Image healthBarImage;
    private Image healthBorderImage;
    private InteractionCharacter characterInteract;
    private TextMeshProUGUI playerNameTextField;


    private void Awake()
    {
        playerNameTextField = transform.Find("GroupMemberHealth").Find("PlayerName").GetComponent<TextMeshProUGUI>();
        healthBarImage = transform.Find("GroupMemberHealth").Find("HealthFill").GetComponent<Image>();
        healthBorderImage = transform.Find("GroupMemberHealth").Find("HealthBorder").GetComponent<Image>();
        characterInteract = transform.parent.parent.parent.parent.GetComponent<InteractionCharacter>();
    }

    private void Start()
    {
        var myColor = healthBarImage.color;
        myColor.a = 0.7f;
        healthBarImage.color = myColor;

        var myBorderColor = healthBorderImage.color;
        myBorderColor.a = 0.6f;
        healthBorderImage.color = myBorderColor;
        GetComponent<Button>().onClick.AddListener(() => { SelectPlayerOnClick(); });
    }
    public void UpdateMyUI()
    {
        playerNameTextField.text = myPlayerName;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        var myColor = healthBarImage.color;
        myColor.a = 1f;
        healthBarImage.color = myColor;

        var myBorderColor = healthBorderImage.color;
        myBorderColor.a = 1f;
        healthBorderImage.color = myBorderColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        var myColor = healthBarImage.color;
        myColor.a = 0.7f;
        healthBarImage.color = myColor;

        var myBorderColor = healthBorderImage.color;
        myBorderColor.a = 0.6f;
        healthBorderImage.color = myBorderColor;
    }

    public void SelectPlayerOnClick()
    {
        Debug.Log("Tryig to set Foxus 1");
        if (mouseOver)
        {
            Debug.Log("Tryig to set Foxus");
            Debug.Log(myPlayerObject);
            characterInteract.SetFocus(myPlayerObject.GetComponent<Interactable>());
        }
    }
}
