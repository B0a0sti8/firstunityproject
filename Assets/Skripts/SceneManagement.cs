using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name; // Get name of current Scene
        Debug.Log("test");
        Debug.Log(currentSceneName);
        
    }
}
