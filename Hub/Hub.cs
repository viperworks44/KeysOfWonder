using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hub : MonoBehaviour
{
    public string sceneName;

    [SerializeField] private GameObject locked2;
    [SerializeField] private GameObject locked3;
    [SerializeField] private GameObject locked4;
    [SerializeField] private GameObject locked5;
    [SerializeField] private GameObject locked6;

    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    [SerializeField] private GameObject button3;
    [SerializeField] private GameObject button4;
    [SerializeField] private GameObject button5;
    [SerializeField] private GameObject button6;

    private void Start()
    {
        CheckUnlockStates();
    }

    private void CheckUnlockStates()
    {
        // button1 is always active
        button1.SetActive(true);

        // Unlock state checks
        if (PlayerPrefs.GetInt("unlock2", 0) == 1)
        {
            locked2.SetActive(false);
            button2.SetActive(true);
        }
        else
        {
            locked2.SetActive(true);
            button2.SetActive(false);
        }

        if (PlayerPrefs.GetInt("unlock3", 0) == 1)
        {
            locked3.SetActive(false);
            button3.SetActive(true);
        }
        else
        {
            locked3.SetActive(true);
            button3.SetActive(false);
        }

        if (PlayerPrefs.GetInt("unlock4", 0) == 1)
        {
            locked4.SetActive(false);
            button4.SetActive(true);
        }
        else
        {
            locked4.SetActive(true);
            button4.SetActive(false);
        }

        if (PlayerPrefs.GetInt("unlock5", 0) == 1)
        {
            locked5.SetActive(false);
            button5.SetActive(true);
        }
        else
        {
            locked5.SetActive(true);
            button5.SetActive(false);
        }

        if (PlayerPrefs.GetInt("unlock6", 0) == 1)
        {
            locked6.SetActive(false);
            button6.SetActive(true);
        }
        else
        {
            locked6.SetActive(true);
            button6.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        // Load the scene called in sceneName
        SceneManager.LoadScene(sceneName);
    }
}

    