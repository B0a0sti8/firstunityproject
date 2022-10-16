using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoESpellIndicator : MonoBehaviour
{
    private float elapsed = 0;
    public float duration;
    public bool isIndicatorActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isIndicatorActive)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= duration)
            { Destroy(gameObject); }
        }
    }
}
