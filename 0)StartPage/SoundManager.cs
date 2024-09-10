using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private bool isMuted;
    private AudioSource[] audioSources;

    private void Awake()
    {
        // Singleton pattern to ensure only one SoundManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Load the saved sound state
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        // Find and update all audio sources in the scene
        UpdateAudioSources();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.mute = isMuted;
        }

        // Save the muted state
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    // Call this when a new scene loads to update the audio sources
    public void UpdateAudioSources()
    {
        audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.mute = isMuted;
        }
    }
}
