using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleManager : MonoBehaviour
{
    public GameObject circlePrefab;
    public EquationGenerator equationGenerator;
    public int numberOfChoices = 3;
    public SnakeMovement snake; // Reference to the SnakeMovement script
    public float safeDistance = 6.0f; // Safe distance from the snake

    private List<Vector2> circlePositions = new List<Vector2>(); // List to keep track of circle positions
    private HashSet<int> usedNumbers = new HashSet<int>(); // Set to keep track of used numbers

    // Define the bounds of the rectangle (same as in the SnakeMovement script)
    private const int minX = -7;
    private const int maxX = 7;
    private const int minY = -3;
    private const int maxY = 4;

    void Start()
    {
        PlaceCircles();
        Destroy(circlePrefab);  // Destroy the original circle prefab instance
    }

    public void PlaceCircles()
    {
        int correctAnswer = equationGenerator.GetCorrectAnswer();
        usedNumbers.Add(correctAnswer); // Add correct answer to the used numbers set
        int correctPosition = Random.Range(0, numberOfChoices);

        for (int i = 0; i < numberOfChoices; i++)
        {
            Vector2 position = GetValidPosition();
            GameObject circle = Instantiate(circlePrefab, position, Quaternion.identity);
            circlePositions.Add(position); // Add position to the list of circle positions
            TextMesh numberText = circle.GetComponentInChildren<TextMesh>();

            int number;
            if (i == correctPosition)
            {
                number = correctAnswer;
            }
            else
            {
                do
                {
                    number = Random.Range(1, 21);
                } while (usedNumbers.Contains(number)); // Ensure the number is unique

                usedNumbers.Add(number); // Add the unique number to the set
            }

            if (numberText != null)
            {
                numberText.text = number.ToString();
            }
            else
            {
                Debug.LogError("TextMesh component not found in Circle prefab!");
            }

            circle.tag = "Circle";  // Ensure the tag is set
        }
    }

    Vector2 GetValidPosition()
    {
        Vector2 newPosition;
        bool validPosition = false;

        do
        {
            newPosition = GetRandomPosition();
            validPosition = IsValidPosition(newPosition);
        } while (!validPosition);

        return newPosition;
    }

    Vector2 GetRandomPosition()
    {
        // Ensure the circles spawn at integer positions within the defined rectangle
        int x = Random.Range(minX, maxX + 1); // maxX + 1 to include maxX as a possible value
        int y = Random.Range(minY, maxY + 1); // maxY + 1 to include maxY as a possible value
        return new Vector2(x, y);
    }

    bool IsValidPosition(Vector2 position)
    {
        // Check distance to the snake's head
        if (Vector2.Distance(position, snake.transform.position) < safeDistance)
        {
            return false;
        }

        // Check distance to each body part
        foreach (Transform bodyPart in snake.bodyParts)
        {
            if (Vector2.Distance(position, bodyPart.position) < safeDistance)
            {
                return false;
            }
        }

        // Check distance to already spawned circles
        foreach (Vector2 circlePosition in circlePositions)
        {
            if (Vector2.Distance(position, circlePosition) < safeDistance)
            {
                return false;
            }
        }

        return true;
    }
}
