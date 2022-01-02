using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 position, int damageAmount, bool isHealing)
    {
        Transform dmgPopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = dmgPopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isHealing);

        return damagePopup;
    }

    private static int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private float disappearTimer;
    private TextMeshPro textMesh;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isHealing)
    {
        textMesh.SetText(damageAmount.ToString());
        textMesh.fontSize = 5;
        if (!isHealing)
        {
            textColor = new Color(1, 1, 0);
        }
        else
        {
            textColor = new Color(0, 1, 0);
        }
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVector = new Vector3(0.2f, 0.3f) * 1f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 0.2f * Time.deltaTime;

        //if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
        //{
        //    float increaseScaleAmount = 1f;
        //    //transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        //}
        //else
        //{
        //    float decreaseScaleAmount = 1f;
        //    //transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        //}
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            // Start disappearing
            float disappearSpeed = 30f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
