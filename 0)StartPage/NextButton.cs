using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButton : MonoBehaviour
{
    public GameObject nextButton; // Reference to your NextButton GameObject

    void Start()
    {
        //nextButton = GetComponent<GameObject>();
        StartCoroutine(ActivateButtonAfterDelay(5f)); // Start the coroutine to activate the button after 5 seconds
    }

    IEnumerator ActivateButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        nextButton.SetActive(true); // Set the button active
    }
}