using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitOnTap : MonoBehaviour
{
    void OnMouseDown()
    {
        // Check if the application is running in a build (not in the Unity Editor)
        #if !UNITY_EDITOR
        PlayerPrefs.SetInt("firstplay", 1);
        Application.Quit();
        #endif
    }
}