using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject[] Levels;
    public AudioSource successaudio;
    public AudioSource defaeataudio;

    private List<GameObject> remainingLevels;
    private int currentLevelIndex;
    private bool hasAnsweredCurrentLevel; // Flag to track if the current level's answer has been handled

    void Start()
    {
        if (!PlayerPrefs.HasKey("unlock4"))
        {
            PlayerPrefs.SetInt("unlock4", 0);
        }

        InitializeLevels();
    }

    // Initialize levels and shuffle them
    private void InitializeLevels()
    {
        remainingLevels = new List<GameObject>(Levels);
        ShuffleLevels();
        currentLevelIndex = 0;
        SetActiveLevel(currentLevelIndex);
        hasAnsweredCurrentLevel = false; // Reset flag for the first level
    }

    // Shuffle the levels
    private void ShuffleLevels()
    {
        for (int i = 0; i < remainingLevels.Count; i++)
        {
            GameObject temp = remainingLevels[i];
            int randomIndex = Random.Range(i, remainingLevels.Count);
            remainingLevels[i] = remainingLevels[randomIndex];
            remainingLevels[randomIndex] = temp;
        }
    }

    private void SetActiveLevel(int index)
    {
        // Deactivate all levels first
        foreach (GameObject level in Levels)
        {
            level.SetActive(false);
        }
        // Activate the level at the given index
        if (index >= 0 && index < Levels.Length)
        {
            Levels[LevelsIndexOf(remainingLevels[index])].SetActive(true);
            hasAnsweredCurrentLevel = false; // Reset flag for the new level
        }
    }

    public void CorrectAnswer()
    {
        if (!hasAnsweredCurrentLevel) // Only allow the answer to be handled if it hasn't been already
        {
            hasAnsweredCurrentLevel = true;
            if(successaudio != null)
            {
                successaudio.Play();
            }
            StartCoroutine(HandleAnswer(true));
        }
    }

    public void IncorrectAnswer()
    {
        if (!hasAnsweredCurrentLevel) // Only allow the answer to be handled if it hasn't been already
        {
            hasAnsweredCurrentLevel = true;
            if(defaeataudio != null)
            {
                defaeataudio.Play();
            }
            StartCoroutine(HandleAnswer(false));
        }
    }

    private IEnumerator HandleAnswer(bool isCorrect)
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        if (isCorrect)
        {
            // Remove the current level from remaining levels
            if (remainingLevels.Count > 1)
            {
                remainingLevels.RemoveAt(currentLevelIndex);
                if (remainingLevels.Count > 0)
                {
                    // Choose a new level randomly from the remaining levels
                    currentLevelIndex = Random.Range(0, remainingLevels.Count);
                    SetActiveLevel(currentLevelIndex);
                }
            }
            else
            {
                // If all levels are completed, set the unlock4 and load the "MultiplePassed" scene
                PlayerPrefs.SetInt("unlock4", 1);
                PlayerPrefs.Save();

                if (PlayerPrefs.GetInt("already3", 0) == 1){
                    SceneManager.LoadScene("MultiplePassedAgain");
                }
                else{
                    PlayerPrefs.SetInt("already3", 1);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("MultiplePassed");
                }
            }
        }
        else
        {
            SceneManager.LoadScene("MultipleFailed");
        }
    }

    // Custom method to find the index of a GameObject in the Levels array
    private int LevelsIndexOf(GameObject go)
    {
        for (int i = 0; i < Levels.Length; i++)
        {
            if (Levels[i] == go)
            {
                return i;
            }
        }
        return -1; // Return -1 if not found
    }
}
