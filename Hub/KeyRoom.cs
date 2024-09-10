using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyRoom : MonoBehaviour
{
    public string sceneName;

    [SerializeField] private GameObject locked1;
    [SerializeField] private GameObject locked2;
    [SerializeField] private GameObject locked3;
    [SerializeField] private GameObject locked4;
    [SerializeField] private GameObject locked5;
    [SerializeField] private GameObject locked6;

    [SerializeField] private GameObject unlocked1;
    [SerializeField] private GameObject unlocked2;
    [SerializeField] private GameObject unlocked3;
    [SerializeField] private GameObject unlocked4;
    [SerializeField] private GameObject unlocked5;
    [SerializeField] private GameObject unlocked6;

    private void Start()
    {
        CheckUnlockStates();
    }

    private void CheckUnlockStates()
    {

        if (PlayerPrefs.GetInt("already1", 0) == 1)
        {
            locked1.SetActive(false);
            unlocked1.SetActive(true);
        }
        else
        {
            locked1.SetActive(true);
            unlocked1.SetActive(false);
        }
        // unlocked state checks
        if (PlayerPrefs.GetInt("already2", 0) == 1)
        {
            locked2.SetActive(false);
            unlocked2.SetActive(true);
        }
        else
        {
            locked2.SetActive(true);
            unlocked2.SetActive(false);
        }

        if (PlayerPrefs.GetInt("already3", 0) == 1)
        {
            locked3.SetActive(false);
            unlocked3.SetActive(true);
        }
        else
        {
            locked3.SetActive(true);
            unlocked3.SetActive(false);
        }

        if (PlayerPrefs.GetInt("already4", 0) == 1)
        {
            locked4.SetActive(false);
            unlocked4.SetActive(true);
        }
        else
        {
            locked4.SetActive(true);
            unlocked4.SetActive(false);
        }

        if (PlayerPrefs.GetInt("already5", 0) == 1)
        {
            locked5.SetActive(false);
            unlocked5.SetActive(true);
        }
        else
        {
            locked5.SetActive(true);
            unlocked5.SetActive(false);
        }

        if (PlayerPrefs.GetInt("already6", 0) == 1)
        {
            locked6.SetActive(false);
            unlocked6.SetActive(true);
        }
        else
        {
            locked6.SetActive(true);
            unlocked6.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        // Load the scene called in sceneName
        SceneManager.LoadScene(sceneName);
    }
}
