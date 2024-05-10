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
    Vector3 originalScale;
    Vector3 orignalPosition;

    Vector3 myCorrectionVector;

    Vector3 dragSnapPos;
    bool isDragging;

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

    public void OnDrag(PointerEventData eventData)
    {
        //Vector2 screen_pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //Vector2 anchorPos = screen_pos - new Vector2(myRect.position.x, myRect.position.y);
        //anchorPos = new Vector2(anchorPos.x / myRect.lossyScale.x, anchorPos.y / myRect.lossyScale.y);

        //if (Mathf.Abs(anchorPos.x) < (myRect.rect.xMax - myRect.rect.xMin) / 2 && Mathf.Abs(anchorPos.y) < (myRect.rect.yMax - myRect.rect.yMin) / 2)
        //{
           
        //}


        DragTalentTree(eventData);
    }

    private void DragTalentTree(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector3 screen_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        Vector3 anchorPos = screen_pos - new Vector3(myRect.position.x, myRect.position.y, myRect.position.z);
        anchorPos = new Vector3(anchorPos.x / myRect.lossyScale.x, anchorPos.y / myRect.lossyScale.y, anchorPos.y / myRect.lossyScale.z);

        if (Mathf.Abs(anchorPos.x) < (myRect.rect.xMax - myRect.rect.xMin) / 2 && Mathf.Abs(anchorPos.y) < (myRect.rect.yMax - myRect.rect.yMin) / 2)
        {
            Vector2 normalizedPoint = Rect.PointToNormalized(myRect.rect, anchorPos);
            Vector3 deltaPosition = myRect.pivot - normalizedPoint;    // get change in pivot
            deltaPosition.Scale(myRect.rect.size);           // apply sizing
            deltaPosition.Scale(myRect.localScale);          // apply scaling
            deltaPosition = myRect.rotation * deltaPosition; // apply rotation

            myRect.localPosition = - deltaPosition;


            isDragging = true;
            //Vector2 localMousePos;
            //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(myRect, eventData.position, eventData.pressEventCamera, out localMousePos))
            //{
            //    //myCorrectionVector = new Vector3(localMousePos.x, localMousePos.y, 0f) - myRect.localPosition;

            //}
        }



        //Vector2 localMousePois;
        //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(myRect, eventData.position, eventData.pressEventCamera, out localMousePois))
        //{
        //    Debug.Log(localMousePois);    // - new Vector3(myRect.pivot.x * myRect.rect.width, myRect.pivot.y * myRect.rect.width, 0f));

        //    myRect.localPosition = (Vector2)localMousePois;// - (Vector2)myCorrectionVector;    // + new Vector3(myRect.pivot.x * myRect.rect.width, myRect.pivot.y * myRect.rect.width, 0f);
        //}
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 screen_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        Vector3 anchorPos = screen_pos - new Vector3(myRect.position.x, myRect.position.y, myRect.position.z);
        anchorPos = new Vector3(anchorPos.x / myRect.lossyScale.x, anchorPos.y / myRect.lossyScale.y, anchorPos.y / myRect.lossyScale.z);

        if (Mathf.Abs(anchorPos.x) < (myRect.rect.xMax - myRect.rect.xMin) / 2 && Mathf.Abs(anchorPos.y) < (myRect.rect.yMax - myRect.rect.yMin) / 2)
        {

            isDragging = true;
            //Vector2 localMousePos;
            //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(myRect, eventData.position, eventData.pressEventCamera, out localMousePos))
            //{
            //    //myCorrectionVector = new Vector3(localMousePos.x, localMousePos.y, 0f) - myRect.localPosition;

            //}
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