using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// inspired by: Brackeys - PAUSE MENU in Unity

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    private void Start()
    {
        //Debug.Log(GameIsPaused);
        pauseMenuUI.SetActive(false);
    }

    // DON'T FORGET TO ADD PLAYER INPUT COMPONENT TO CANVAS! (where this skript is)
    // with the correct default map (Overlay)
    private void OnPauseMenu(InputValue value)
    {
        Debug.Log("Pause Button Pressed");
        if (GameIsPaused)
        {
            Resume();
        } else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // close Pause Menu
        Time.timeScale = 1f; // make time normal
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // open Pause Menu
        Time.timeScale = 0f; // freeze time
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); // maybe change "Menu" to something not hardcoded (e.g. 0)
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }


    // for audio skripts for example to change sound while paused
    //if (PauseMenu.GameIsPaused) 
    //{
    //    s.source.pitch *= .5f;
    //}
}
