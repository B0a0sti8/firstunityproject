using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    private IUseable useable;

    public Button MyButton { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if(useable != null)
        {
            useable.Use();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
  
    }
}
