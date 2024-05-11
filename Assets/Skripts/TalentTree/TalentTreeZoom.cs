using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TalentTreeZoom : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] TextMeshProUGUI myTextMesh;
    [SerializeField] TextMeshProUGUI myTextMeshRect;
    RectTransform myRect;
    RectTransform myMask;
    Vector3 originalScale;
    Vector3 orignalPosition;

    Vector3 myCorrectionVector;

    Vector3 dragSnapPos;
    bool isDragging;

    private void Start()
    {
        myRect = GetComponent<RectTransform>();
        myMask = transform.parent.GetComponent<RectTransform>();
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

    public void OnDrag(PointerEventData eventData)
    {
        DragTalentTree(eventData);
    }

    private void DragTalentTree(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector3 worldMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(myRect, eventData.position, eventData.pressEventCamera, out worldMousePos))
        {
            //Debug.Log(worldMousePos);    // - new Vector3(myRect.pivot.x * myRect.rect.width, myRect.pivot.y * myRect.rect.width, 0f));
            myRect.position = (Vector3)worldMousePos + myCorrectionVector;// - (Vector2)myCorrectionVector;    // + new Vector3(myRect.pivot.x * myRect.rect.width, myRect.pivot.y * myRect.rect.width, 0f);
        }

        Vector3[] myRectWorldCorners = new Vector3[4];
        Vector3[] myMaskWorldCorners = new Vector3[4];

        myRect.GetWorldCorners(myRectWorldCorners);
        myMask.GetWorldCorners(myMaskWorldCorners);

        if (myMaskWorldCorners[0].x < myRectWorldCorners[0].x) myRect.position += new Vector3(myMaskWorldCorners[0].x - myRectWorldCorners[0].x, 0, 0);
        if (myMaskWorldCorners[2].x > myRectWorldCorners[2].x) myRect.position += new Vector3(myMaskWorldCorners[2].x - myRectWorldCorners[2].x, 0, 0);
        if (myMaskWorldCorners[3].y < myRectWorldCorners[3].y) myRect.position += new Vector3(0, myMaskWorldCorners[3].y - myRectWorldCorners[3].y, 0);
        if (myMaskWorldCorners[1].y > myRectWorldCorners[1].y) myRect.position += new Vector3(0, myMaskWorldCorners[1].y - myRectWorldCorners[1].y, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 worldMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(myRect, eventData.position, eventData.pressEventCamera, out worldMousePos))
        {
            myCorrectionVector = myRect.position - worldMousePos;
            isDragging = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
}


// 1270 für loS = 4
// 800 für los = 3
// 370 für los = 2
// 0 für los = 1
// 