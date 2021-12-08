using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMeter : MonoBehaviour
{
    public float totalDamage = 0f;
    public float meterTime = 0f;
    float viewEveryXSeconds = 1f;
    float viewEveryXSecondsTimer = 0f;
    public bool startDPStracking = false;

    private void OnDPSMeterReset()
    {
        totalDamage = 0f;
        meterTime = 0f;
        viewEveryXSecondsTimer = 0f;
        startDPStracking = true;
    }

    void Update()
    {
        if (startDPStracking)
        {
            meterTime += Time.deltaTime;
            viewEveryXSecondsTimer += Time.deltaTime;
            if (viewEveryXSecondsTimer >= viewEveryXSeconds)
            {
                viewEveryXSecondsTimer = 0f;
                // DPS = totalDamage / meterTime;
                gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text =
                    "DPS: " + (Mathf.Round((totalDamage / meterTime) * 10) / 10.0).ToString("0.0").Replace(",", ".");

            }
        }
    }
}
