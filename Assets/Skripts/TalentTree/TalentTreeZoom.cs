using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TalentTreeZoom : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI myTextMesh;
    [SerializeField] TextMeshProUGUI myTextMeshRect;
    RectTransform myRect;
    Vector3 originalScale;
    Vector3 orignalPosition;

    private void Start()
    {
        myRect = GetComponent<RectTransform>();
        originalScale = myRect.localScale;
        orignalPosition = myRect.localPosition;
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            ComputeMousePosition();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            ComputeMousePosition();
        }
    }

    void SetPivot(Vector2 pivot)
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            Vector3 deltaPosition = myRect.pivot - pivot;    // get change in pivot
            deltaPosition.Scale(myRect.rect.size);           // apply sizing
            deltaPosition.Scale(myRect.localScale);          // apply scaling
            deltaPosition = myRect.rotation * deltaPosition; // apply rotation

            myRect.pivot = pivot;                            // change the pivot
            myRect.localPosition -= deltaPosition;           // reverse the position change

            Vector3 oldScale = myRect.localScale;
            if (oldScale.x < 4 && oldScale.y < 4)
            {
                myRect.localScale += new Vector3(0.1f, 0.1f, 0f);
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            pivot = new Vector2(0.5f, 0.5f);
            Vector3 deltaPosition = myRect.pivot - pivot;    // get change in pivot
            deltaPosition.Scale(myRect.rect.size);           // apply sizing
            deltaPosition.Scale(myRect.localScale);          // apply scaling
            deltaPosition = myRect.rotation * deltaPosition; // apply rotation

            myRect.pivot = pivot;                            // change the pivot
            myRect.localPosition -= deltaPosition;           // reverse the position change

            Vector3 oldScale = myRect.localScale;
            if (oldScale.x > 1.09 && oldScale.y > 1.09)
            {
                myRect.localScale -= new Vector3(0.1f, 0.1f, 0f);
                int stepCount = (int)((oldScale.x - originalScale.x) / 0.1f);
                if (stepCount > 0)
                {
                    Vector3 smallStep = (orignalPosition - myRect.localPosition) / stepCount;
                    myRect.localPosition += smallStep;
                }
            }
        }
    }

    void ComputeMousePosition()
    {
        Debug.Log("ComputingMouse");
        Vector2 screen_pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 anchorPos = screen_pos - new Vector2(myRect.position.x, myRect.position.y);
        anchorPos = new Vector2(anchorPos.x / myRect.lossyScale.x, anchorPos.y / myRect.lossyScale.y);

        myTextMesh.text = anchorPos.ToString();
        myTextMeshRect.text = (myRect.rect.xMax).ToString() + "/" +  (myRect.rect.yMax).ToString();

        Debug.Log("Checking if Mouse in Rect");
        if (Mathf.Abs(anchorPos.x) < (myRect.rect.xMax - myRect.rect.xMin) / 2 && Mathf.Abs(anchorPos.y) < (myRect.rect.yMax - myRect.rect.yMin) / 2)
        {

            
            Debug.Log("Mouse in Rect");
            Vector2 normalizedPoint = Rect.PointToNormalized(myRect.rect, anchorPos);
            SetPivot(normalizedPoint);
        }
    }
}
