using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    public const int columns = 4;
    public const int rows = 2;

    public const float Xspace = 4f;
    public const float Yspace = -4f;

    [SerializeField] private MainImageScript startObject;
    [SerializeField] private Sprite[] images;

    [SerializeField] private AudioSource audioSource;  // Reference to the AudioSource component
    [SerializeField] private AudioClip matchSound;     // Reference to the AudioClip for matching sound

    private int totalPairs;
    private int matchedPairs;

    private int[] Randomiser(int[] locations)
    {
        int[] array = locations.Clone() as int[];
        for (int i = 0; i < array.Length; i++)
        {
            int newArray = array[i];
            int j = Random.Range(i, array.Length);
            array[i] = array[j];
            array[j] = newArray;
        }
        return array;
    }

    private void Start()
    {
        totalPairs = columns * rows / 2;
        matchedPairs = 0;

        int[] locations = { 0, 0, 1, 1, 2, 2, 3, 3 };
        locations = Randomiser(locations);

        Vector3 startPosition = startObject.transform.position;

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                MainImageScript gameImage;
                if (i == 0 && j == 0)
                {
                    gameImage = startObject;
                }
                else
                {
                    gameImage = Instantiate(startObject) as MainImageScript;
                }

                int index = j * columns + i;
                int id = locations[index];
                gameImage.ChangeSprite(id, images[id]);

                float positionX = (Xspace * i) + startPosition.x;
                float positionY = (Yspace * j) + startPosition.y;

                gameImage.transform.position = new Vector3(positionX, positionY, startPosition.z);
            }
        }
    }

    private MainImageScript firstOpen;
    private MainImageScript secondOpen;

    public bool canOpen
    {
        get { return secondOpen == null; }
    }

    public void imageOpened(MainImageScript startObject)
    {
        if (firstOpen == null)
        {
            firstOpen = startObject;
        }
        else
        {
            secondOpen = startObject;
            StartCoroutine(CheckGuessed());
        }
    }

    private IEnumerator CheckGuessed()
    {
        if (firstOpen.spriteId == secondOpen.spriteId)
        {
            // Play match sound
            if (audioSource != null && matchSound != null)
            {
                audioSource.PlayOneShot(matchSound);
            }

            matchedPairs++;
            if (matchedPairs == totalPairs)
            {
                // Unlock and save the state
                PlayerPrefs.SetInt("unlock2", 1);
                PlayerPrefs.Save();
                yield return new WaitForSeconds(1.25f);
                // Load the next scene
                if (PlayerPrefs.GetInt("already1", 0) == 1){
                    SceneManager.LoadScene("MemoryPassedAgain");
                }
                else{
                    PlayerPrefs.SetInt("already1", 1);
                    PlayerPrefs.SetInt("firstplay", 0);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("MemoryPassed");
                }
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f); // timer

            firstOpen.Close();
            secondOpen.Close();
        }

        firstOpen = null;
        secondOpen = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene("MemoryGame");
    }
}
