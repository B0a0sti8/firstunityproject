using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// inspired by: Brackeys - SETTINGS MENU in Unity

public class OptionsMenue : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMPro.TMP_Dropdown resolutionDropdown; // use TMPro.TMP_Dropdown insted of Dropdown

    public TMPro.TMP_Text mainValue;
    public TMPro.TMP_Text musicValue;
    public TMPro.TMP_Text soundeffectsValue;

    Resolution[] resolutions; // [] means array

    void Start() // maybe with private void Start?
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>(); // create empty List of strings (array has fixed size - size of list can be changed)

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // select the correct resolution
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options); // add individual resolutions to dropdown
        // AddOption takes in a list of strings and not an array of resolutions
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue(); // display resolution
        
        // mainValue.SetText("80");
        // musicValue.SetText("80");
        // soundeffectsValue.SetText("80");
    }

    // unpdate resolution changes
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolumeMain(float volume)
    {
        // Debug.Log("Set volume to: " + volume);
        audioMixer.SetFloat("volume", volume); // "volume" = name of exposed parameter
        mainValue.SetText((volume + 80).ToString());
    }

    public void SetVolumeMusic(float volume)
    {
        // Debug.Log("Set volume to: " + volume);
        audioMixer.SetFloat("volumeMusic", volume); // "volumeMusic" = name of exposed parameter
        musicValue.SetText((volume + 80).ToString());
    }

    public void SetVolumeSoundeffects(float volume)
    {
        // Debug.Log("Set volume to: " + volume);
        audioMixer.SetFloat("volumeSoundeffects", volume); // "volumeSoundeffect" = name of exposed parameter
        soundeffectsValue.SetText((volume + 80).ToString());
    }

    public void SetQuality(int qualityIndex) // int 0 for "Very Low", 1 for "Low", ... 5 for "Ultra"
    {
        Debug.Log(qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Debug.Log(isFullscreen);
        Screen.fullScreen = isFullscreen;
    }

    // Resizeable Window: Edit > Project Settings > Player > Resolution and Presentation > Resizeable Window

}
