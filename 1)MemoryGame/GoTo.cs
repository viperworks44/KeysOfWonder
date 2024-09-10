using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoTo : MonoBehaviour
{   
    private AudioSource audioSource;

    public string sceneName;

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

        // Load the scene 
        PlayerPrefs.SetInt("CurrentRound", 0);
        SceneManager.LoadScene(sceneName);
        
    }

}

