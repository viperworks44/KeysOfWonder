using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Allover : MonoBehaviour
{   
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to this object
        audioSource = GetComponent<AudioSource>();
    }
    void OnMouseDown()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        
        StartCoroutine(LoadSceneAfterSound());
    }

    IEnumerator LoadSceneAfterSound()
    {
        // Wait until the audio clip finishes playing
        if (audioSource != null)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        // Load the scene called "StartPage"
        SceneManager.LoadScene("StartPage");
    }
}