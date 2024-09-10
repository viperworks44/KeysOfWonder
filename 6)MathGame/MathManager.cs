using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class MathManager : MonoBehaviour
{
    public int totalRounds = 5; // Total number of rounds to complete
    public int currentRound; // Track current round

    public TextMesh levelTextMesh; // Level Text

    public GameObject readyObject; // Reference to the "Ready" object
    public GameObject goObject; // Reference to the "Go!" object
    public GameObject[] roundObjects; // Array of objects representing rounds

    public AudioSource gamestart;

    void Start()
    {   
        // Load the current round from PlayerPrefs
        currentRound = PlayerPrefs.GetInt("CurrentRound", 0);

        // Continue with the rest of the game logic
        UpdateLevelText();

        // Freeze the game initially
        Time.timeScale = 0f;

        // Start the round with the "Ready - Go!" sequence
        StartCoroutine(StartRoundSequence());
    }

    IEnumerator StartRoundSequence()
    {   
        // Show the "Ready" object
        readyObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2f); // Wait for 2 real-time seconds
        readyObject.SetActive(false);
        
        // Show the "Go!" object
        goObject.SetActive(true);
        if (gamestart != null)
        {
            gamestart.Play(); // Play the attached sound effect
        }
        yield return new WaitForSecondsRealtime(1f); // Wait for 1 real-time second
        goObject.SetActive(false);

        // Unfreeze the game after the sequence
        Time.timeScale = 1f;

        // Activate the round object if you're using different objects per round
        if (roundObjects != null && roundObjects.Length > currentRound)
        {
            roundObjects[currentRound].SetActive(true);
        }

        
    }

    public void LoadNextLevel()
    {

        currentRound++; // Increment the current round
        PlayerPrefs.SetInt("CurrentRound", currentRound); // Save the current round

        if (currentRound >= totalRounds)
        {
            ShowCongratulations(); // Show congratulations message if all rounds are completed
        }
        else
        {
            // Reload the current scene to restart the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GameOver()
    {
        // Load the "MathFailed" scene
        SceneManager.LoadScene("MathFailed");
    }

    private void ShowCongratulations()
    {
        // Load the "MathPassed" scene
        if (PlayerPrefs.GetInt("already6", 0) == 1)
        {
            SceneManager.LoadScene("MathPassedAgain");
        }
        else
        {
            PlayerPrefs.SetInt("already6", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("MathPassed");
        }
    }

    public void UpdateLevelText()
    {
        if (levelTextMesh != null)
        {
            levelTextMesh.text = "Round: " + (currentRound + 1) + "/" + totalRounds;
        }
    }

    public void ResetGame()
    {
        // Reset the current round to 0 and save it
        currentRound = 0;
        PlayerPrefs.SetInt("CurrentRound", currentRound);
        Time.timeScale = 1f;
        // Reload the scene to start over
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {   
        PlayerPrefs.SetInt("CurrentRound", currentRound);
    }
}
