using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.Netcode;

// Im Multiplayer muss man die Kamera am Anfang deaktiviert lassen und erst aktivieren wenn der Spieler spawnt.


public class CameraFollow : NetworkBehaviour
{
    GameObject cameera;
    GameObject CameraMama;
    GameObject vcam1;
    bool isMouseOverGameObject;

    private void Start()
    {
        if (IsOwner)
        {
            GameObject prevMainCam = GameObject.Find("Main Camera");
            if (prevMainCam != null)
            {
                prevMainCam.SetActive(false);
            }
        }
        
        CameraMama = GameObject.Find("CameraMama");
        CameraMama.transform.GetChild(0).gameObject.SetActive(true);

        vcam1 = CameraMama.transform.GetChild(0).transform.GetChild(0).gameObject;
        vcam1.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize = 6.5f;
    }

    private void Update()
    {
        if (!IsOwner) { return; }

        vcam1.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = gameObject.transform;

        isMouseOverGameObject = MouseOverGameObj();
    }

    bool MouseOverGameObj()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return true;
        else return false;

    }

    public void CameraZoom()
    {
        if (isMouseOverGameObject) return; // no zoom when mouse over UI element

        Vector2 vec = Mouse.current.scroll.ReadValue(); // either 120 and 0, or -120 and 0

        if (vec.y > 0)
        {
            //lensZoom += 0.5f;
            vcam1.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize -= 0.25f;
        }
        else if (vec.y < 0)
        {
            //lensZoom -= 0.5f;
            vcam1.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize += 0.25f;
        }

        if (vcam1.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize > 13f)
        {
            //lensZoom = 13;
            vcam1.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize = 13f;
        }
        else if (vcam1.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize < 3f)
        {
            //lensZoom = 3;
            vcam1.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize = 3f;
        }

    }
}