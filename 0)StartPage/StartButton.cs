using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to this object
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        // Play the sound
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Save player preferences
        PlayerPrefs.SetInt("unlock2", 0);
        PlayerPrefs.SetInt("unlock3", 0);
        PlayerPrefs.SetInt("unlock4", 0);
        PlayerPrefs.SetInt("unlock5", 0);
        PlayerPrefs.SetInt("unlock6", 0);

        PlayerPrefs.SetInt("already1", 0);
        PlayerPrefs.SetInt("already2", 0);
        PlayerPrefs.SetInt("already3", 0);
        PlayerPrefs.SetInt("already4", 0);
        PlayerPrefs.SetInt("already5", 0);
        PlayerPrefs.SetInt("already6", 0);
        
        PlayerPrefs.SetInt("firstplay", 1);

        PlayerPrefs.Save();

        // Start the coroutine to load the scene after a delay
        StartCoroutine(LoadSceneAfterSound());
    }

    IEnumerator LoadSceneAfterSound()
    {
        // Wait until the audio clip finishes playing
        if (audioSource != null)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        // Load the scene called "StartPlot"
        SceneManager.LoadScene("StartPlot");
    }
}