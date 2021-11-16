using UnityEngine.Audio;
using UnityEngine;

// inspired by: Brackeys - Introduction to AUDIO in Unity

// whenever you create a custom class and you want this class to
// appear in the inspector, you have to mark it as serializable
[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    public AudioMixerGroup output; // reference here the AudioMixer Master

    [Range(0f, 1f)] // make Slider
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source; // store AudioSource in a variable
}
