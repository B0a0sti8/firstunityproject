using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCharacterButtonUI : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => { SendButtonLoadInfo(); });
    }

    private void SendButtonLoadInfo()
    {
        transform.parent.parent.parent.GetComponent<LoadOrNewCharacterUI>().LoadCharacterOnButtonPress(transform);
    }
}
