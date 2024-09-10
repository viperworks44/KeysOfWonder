using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShapeShadowManager : MonoBehaviour
{   
    [System.Serializable]
    public class ShapeShadowPair
    {
        public GameObject shape;
        public GameObject shadow;
    }

    public ShapeShadowPair[] shapeShadowPairs;

    private int placedPiecesCounter = 0;

    void Start()
    {
        // Ensure all shapes start in their initial positions
        foreach (var pair in shapeShadowPairs)
        {
            pair.shape.GetComponent<MoveSystem>().Initialize(pair.shadow);
        }
    }

    public void IncrementCounter()
    {   
        placedPiecesCounter++;
        // Check if all pieces are placed correctly
        if (placedPiecesCounter == shapeShadowPairs.Length)
        {   
            StartCoroutine(Next());           
        }
    }

    private IEnumerator Next()
    {
        yield return new WaitForSeconds(1.5f);
        // Set the unlock5 boolean and load the "SortingPassed" scene
        PlayerPrefs.SetInt("unlock5", 1);
        PlayerPrefs.Save();
        if (PlayerPrefs.GetInt("already4", 0) == 1){
                SceneManager.LoadScene("SortingPassedAgain");
        }
        else{
            PlayerPrefs.SetInt("already4", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("SortingPassed");
        } 
    }

}