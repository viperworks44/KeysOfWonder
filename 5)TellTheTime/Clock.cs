using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    public RectTransform hourHand;
    public RectTransform minuteHand;
    public TextMesh feedbackText;
    public Camera uiCamera;
    public TextMesh watch;

    // Cables
    public GameObject[] bareCables; // Array of bare cables
    public GameObject[] greenCables; // Array of green cables
    public GameObject[] redCables; // Array of red cables

    // Sounds
    public AudioSource successSound; // Audio source for success sound
    public AudioSource errorSound; // Audio source for error sound

    private bool isDraggingMinute = false;

    private float targetHourAngle;
    private float targetMinuteAngle;

    private float totalMinuteRotation = 0;

    private int targetHour;
    private int targetMinute;

    private int currentLevel = 0; // Change to 0 because arrays are 0-indexed
    private int totalLevels = 5;

    private void Start()
    {
        SetRandomTargetTime();
        UpdateWatchDisplay();

        if (!PlayerPrefs.HasKey("unlock6"))
        {
            PlayerPrefs.SetInt("unlock6", 0);
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            if (RectTransformUtility.RectangleContainsScreenPoint(minuteHand, mousePosition, uiCamera))
            {
                isDraggingMinute = true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (isDraggingMinute)
            {
                RotateHand(minuteHand, false);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDraggingMinute = false;
        }
    }

    public void RotateHand(RectTransform hand, bool isHourHand)
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 handPosition = RectTransformUtility.WorldToScreenPoint(uiCamera, hand.position);
        Vector2 direction = mousePosition - handPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        if (!isHourHand)
        {
            float previousAngle = minuteHand.localEulerAngles.z;
            float rotationDifference = Mathf.DeltaAngle(previousAngle, angle);
            totalMinuteRotation += rotationDifference;

            minuteHand.rotation = Quaternion.Euler(0, 0, angle);

            float hourRotation = (totalMinuteRotation / 360) * 30;
            hourHand.rotation = Quaternion.Euler(0, 0, hourRotation);
        }
    }

    public void CheckTime()
    {
        float currentHourAngle = NormalizeAngle(hourHand.localEulerAngles.z);
        float currentMinuteAngle = NormalizeAngle(minuteHand.localEulerAngles.z);

        bool hourCorrect = IsAngleWithinRange(currentHourAngle, targetHourAngle - 15f, targetHourAngle + 15f);
        bool minuteCorrect = IsAngleWithinRange(currentMinuteAngle, targetMinuteAngle - 18f, targetMinuteAngle + 18f);

        if (hourCorrect && minuteCorrect)
        {
            feedbackText.text = "Correct! Move to the next level!";
            successSound.Play(); // Play success sound
            ActivateGreenCable();
            if (currentLevel < totalLevels - 1)
            {
                StartCoroutine(ClearFeedbackTextAfterDelay());
                currentLevel++;
                ResetLevel();
            }
            else
            {
                feedbackText.text = "You've completed all levels!";
                PlayerPrefs.SetInt("unlock6", 1);
                PlayerPrefs.Save();
                StartCoroutine(LoadNextSceneAfterDelay(2f));
            }
        }
        else
        {
            feedbackText.text = "Try again!";
            errorSound.Play(); // Play error sound
            ActivateRedCable();
            StartCoroutine(ClearFeedbackTextAfterDelay());
        }
    }

    private void ActivateGreenCable()
    {
        if (currentLevel < greenCables.Length)
        {
            greenCables[currentLevel].SetActive(true);
            bareCables[currentLevel].SetActive(false);
        }
    }

    private void ActivateRedCable()
    {
        if (currentLevel < redCables.Length)
        {
            redCables[currentLevel].SetActive(true);
            StartCoroutine(DeactivateRedCableAfterDelay(2f));
        }
    }

    private IEnumerator DeactivateRedCableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        redCables[currentLevel].SetActive(false);
    }

    private void ResetLevel()
    {
        totalMinuteRotation = 0;
        minuteHand.rotation = Quaternion.Euler(0, 0, 0);
        hourHand.rotation = Quaternion.Euler(0, 0, 0);
        SetRandomTargetTime();
    }

    private IEnumerator ClearFeedbackTextAfterDelay()
    {
        yield return new WaitForSeconds(3);
        feedbackText.text = "";
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (PlayerPrefs.GetInt("already5", 0) == 1){
            SceneManager.LoadScene("TellTheTimePassedAgain");
        }
        else{
            PlayerPrefs.SetInt("already5", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("TellTheTimePassed");
        }
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle < 0) angle += 360;
        return angle;
    }

    private bool IsAngleWithinRange(float angle, float min, float max)
    {
        angle = NormalizeAngle(angle);
        min = NormalizeAngle(min);
        max = NormalizeAngle(max);

        if (min < max)
        {
            return angle >= min && angle <= max;
        }
        else
        {
            return angle >= min || angle <= max;
        }
    }

    private void SetRandomTargetTime()
    {
        targetHour = Random.Range(0, 24);
        targetMinute = Random.Range(0, 12) * 5;

        SetTargetTime(targetHour, targetMinute);
    }

    public void SetTargetTime(int hour, int minute)
    {
        minute = Mathf.RoundToInt(minute / 5f) * 5;
        targetHourAngle = ((hour % 12) + (minute / 60f)) * -30;
        targetMinuteAngle = (minute % 60) * -6;
        UpdateWatchDisplay();
    }

    private void UpdateWatchDisplay()
    {
        if (watch != null)
        {
            watch.text = string.Format("{0:D2}:{1:D2}", targetHour, targetMinute);
        }
    }

    private void OnValidate()
    {
        SetTargetTime(targetHour, targetMinute);
    }
}