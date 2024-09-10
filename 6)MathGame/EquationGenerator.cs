using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationGenerator : MonoBehaviour
{
    public TextMesh equationText;
    private int _correctAnswer;
    public int round = 0;
    public int totalRounds = 5;

    void Start()
    {
        GenerateNewEquation();
    }

    public void GenerateNewEquation()
    {
        if (round >= totalRounds)
        {
            // Game won
            Debug.Log("You win!");
            return;
        }
        
        int a = Random.Range(1, 11);
        int b = Random.Range(1, 11);
        _correctAnswer = a + b;
        equationText.text = $"{a} + {b} = ?";
        round++;
    }

    public int GetCorrectAnswer()
    {
        return _correctAnswer;
    }
}