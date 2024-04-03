using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// inspired by: Brackeys - START MENU in Unity

public class MainMenue : MonoBehaviour
{
    public void PlayGame()
    {
        //FindObjectOfType<AudioManager>().Play("Click"); // click-sound when pressing the button (optional)
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // load next level in the queue
        // or load special scene with the name: SceneManager.LoadScene("Level_XY");
        // or with the build-index: SceneManager.LoadScene(1);

        SceneManager.LoadScene("StartLobby");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
