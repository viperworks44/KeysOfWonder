using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuMath : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuMath;
    public int totalRounds = 5; // Total number of rounds to complete
    public int currentRound; // Track current round
    public TextMesh levelTextMesh; // Reference to the TextMesh component

    public void Pause()
    {
        pauseMenuMath.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenuMath.SetActive(false);
        Time.timeScale = 1f;
    }


    public void Restart()
    {
        Time.timeScale = 1f;
        currentRound = 0;
        PlayerPrefs.SetInt("CurrentRound", 0);
        UpdateLevelText();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void Quit()
    {   
        Time.timeScale = 1f;
        SceneManager.LoadScene("Hub");
    }

    public void UpdateLevelText()
    {
        if (levelTextMesh != null)
        {
            levelTextMesh.text = "Round: " + (currentRound + 1) + "/" + totalRounds;
        }
    }
}