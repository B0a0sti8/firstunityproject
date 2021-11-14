using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string SceneName;
    public Animator transition;
    public float transitionTime = 1f;
    private void OnChangeScene(InputValue value)
    {
        Debug.Log("Wechsle Scene");
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneName));
    }

    IEnumerator LoadLevel(string SceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(SceneName);
    }

}
