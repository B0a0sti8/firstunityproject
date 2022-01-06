using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LevelLoader : MonoBehaviour
{
    GameObject[] allPlayer;
    public Vector3 newPlayerPosition;

    public string SceneName;
    public Animator transition;
    public float transitionTime = 1f;

    string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name; // Get name of current Scene
        Debug.Log("Current Scene: " + currentSceneName);

        allPlayer = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject allP in allPlayer)
        {
            allP.transform.position = newPlayerPosition;
        }
    }


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void ChangeScene() // when pressing O
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Wechsle Scene");
            LoadNextLevel();
        }
        else
        {
            Debug.Log("Only the host can travel!");
        }
    }

    public void GoToHub() // when pressing H
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Go to Hub");
            StartCoroutine(LoadLevel("Sanctuary"));
        }
        else
        {
            Debug.Log("Only the host can travel!");
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneName));
    }

    IEnumerator LoadLevel(string SceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        PhotonNetwork.LoadLevel(SceneName);
    }
}
