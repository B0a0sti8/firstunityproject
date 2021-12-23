using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

// Im Multiplayer muss man die Kamera am Anfang deaktiviert lassen und erst aktivieren wenn der Spieler spawnt.
// Photon: view.IsMine als Unterscheidungsoption zwischen Spielern.

public class CameraFollow : MonoBehaviour
{
    GameObject cameera;
    public GameObject CameraMama;
    PhotonView view;
    
    public GameObject vcam1;
    //public float lensZoom;

    private void Start()
    {
        CameraMama = GameObject.Find("CameraMama");
        CameraMama.transform.GetChild(0).gameObject.SetActive(true);

        cameera = GameObject.Find("CM vcam1"); // !!
        view = GetComponent<PhotonView>();

        vcam1 = CameraMama.transform.GetChild(0).transform.GetChild(0).gameObject;
        //lensZoom = vcam1.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize;
        vcam1.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize = 6.5f;
    }

    private void Update()
    {
        if (view.IsMine)
        {
            cameera = GameObject.Find("CM vcam1");
            cameera.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = gameObject.transform;
        }
    }


    void OnZoom()
    {
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

        //Debug.Log("Zoom Value: " + vcam1.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize);
    }
}