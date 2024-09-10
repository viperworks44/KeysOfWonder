using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTime : MonoBehaviour
{
    public GameObject Starting;
    public GameObject NewGame;
    public GameObject Resume;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the PlayerPref "firstplay" exists
        if (!PlayerPrefs.HasKey("firstplay"))
        {
            // If it doesn't exist, set it to 1 (first play)
            PlayerPrefs.SetInt("firstplay", 1);
            PlayerPrefs.Save();
        }

        // Get the value of "firstplay"
        int firstPlay = PlayerPrefs.GetInt("firstplay");

        // If firstplay is 1, show Start and hide NewGame and Resume
        if (firstPlay == 1)
        {
            Starting.SetActive(true);
            NewGame.SetActive(false);
            Resume.SetActive(false);
        }
        // If firstplay is 0, hide Start and show NewGame, hide Resume
        else if (firstPlay == 0)
        {
            Starting.SetActive(false);
            NewGame.SetActive(true);
            Resume.SetActive(true);
        }
    }
}