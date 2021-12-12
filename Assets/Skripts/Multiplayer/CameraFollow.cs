using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Im Multiplayer muss man die Kamera am Anfang deaktiviert lassen und erst aktivieren wenn der Spieler spawnt.
// Photon: view.IsMine als Unterscheidungsoption zwischen Spielern.

public class CameraFollow : MonoBehaviour
{
    GameObject cameera;
    public GameObject CameraMama;
    PhotonView view;

    private void Start()
    {
        CameraMama = GameObject.Find("CameraMama");
        CameraMama.transform.GetChild(0).gameObject.SetActive(true);

        cameera = GameObject.Find("CM vcam1");
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            cameera = GameObject.Find("CM vcam1");
            cameera.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = gameObject.transform;
        }
    }
}