using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public delegate void KillConfirmed(CharacterStats characterStats);
public class StuffManagerScript : MonoBehaviour
{
    public event KillConfirmed killConfirmedEvent;

    [SerializeField] private GameObject messagePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnKillConfirmed(CharacterStats characterStats)
    {
        if (killConfirmedEvent != null)
        {
            killConfirmedEvent(characterStats);
        }
    }

    public void WriteMessage(string message)
    {
        Debug.Log("MessageWrite");
        GameObject go = Instantiate(messagePrefab, transform.Find("Own Canvases").Find("CanvasQuestUI").Find("MessageFeed"));
        go.GetComponent<TextMeshProUGUI>().text = message;

        go.transform.SetAsFirstSibling();

        Destroy(go, 2);
    }
}
