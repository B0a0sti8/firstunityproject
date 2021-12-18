using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SchmoinkiDoink : MonoBehaviourPunCallbacks, IPunObservable
{
    
    public float Yoinki = 1f;
    public GameObject textFeld;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    // Update is called once per frame
    void Update()
    {
        Yoinki += 1;
        textFeld.GetComponent<Text>().text = Yoinki.ToString();
    }
}
