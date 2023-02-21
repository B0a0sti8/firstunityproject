using UnityEngine.Audio;
using System; // for public void play
using UnityEngine;

// inspired by: Brackeys - Introduction to AUDIO in Unity

// also interesting: Unity - Audio Mixer and Audio Mixer Groups - Unity Official Tutorials

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance; // static reference to the current instance of the AudioManager in the scene

    // Awake is called before Start
    void Awake()
    {
        // there should be only one instance of the AudioManager
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return; // make sure no more code is called, before destroying the object
        }

        DontDestroyOnLoad(gameObject); // makes the gameObject (AudioManager) persist between scenes

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.outputAudioMixerGroup = s.output;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // makes the sound options changable ingame
    // maybe for making sound-changes outside of the AudioMixer
    // copys the values of the audioManager to the AudioSource
    //void Update()
    //{
    //    foreach (Sound s in sounds)
    //    {
    //        s.source.volume = s.volume;
    //        s.source.pitch = s.pitch;
    //        s.source.loop = s.loop;
    //    }
    //}

    // play music (main theme) - could be played form every skript (but sound related skript is more fitting)
    void Start()
    {
        Play("DragonTheme");
    }

    public void Play (string name)
    {
        // loop through all sounds to find the sound with the target name
        // create foreach method and check the name for each element, or:
        // find sound in sounds-array. find sound, where sound.name is equal to name (and store found sound in s)
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) // error if name is spelled wrong
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }


    // reference to AudioManager (no "using UnityEngine.Audio" (or so) required)
    // put in skript where needed (e.g. "Player" skript in OnTakeDamage function)
    // FindObjectOfType<AudioManager>().Play("PlayerDeath");
}
