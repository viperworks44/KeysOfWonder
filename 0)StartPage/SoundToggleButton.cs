using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToggleManager : MonoBehaviour
{
    public static SoundToggleManager instance;
    public GameObject soundOnLogo;  // The object representing sound on
    public GameObject soundOffLogo; // The object representing sound off

    private bool isMuted;
    private AudioSource[] audioSources;

    private void Awake()
    {
        /*// Singleton pattern to ensure only one SoundToggleManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }*/

        // Load the saved sound state
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        // Find and update all audio sources in the scene
        UpdateAudioSources();
        UpdateLogos();
    }

    private void UpdateLogos()
    {
        if (isMuted)
        {
            soundOnLogo.SetActive(false);
            soundOffLogo.SetActive(true);
        }
        else
        {
            soundOnLogo.SetActive(true);
            soundOffLogo.SetActive(false);
        }
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

        // Update the logos to reflect the new sound state
        UpdateLogos();
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

    private void OnMouseDown()
    {
        ToggleSound();
    }
}
