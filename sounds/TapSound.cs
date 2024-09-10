using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to this object
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        // Debug log to check if the method is being called
        Debug.Log("Object tapped!");

        // Play the sound when the object is tapped/clicked
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}