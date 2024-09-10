using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeMovement : MonoBehaviour
{
    public float moveSpeed = 0.1f; // Time in seconds between each movement
    public float directionChangeCooldown = 0.2f; // Cooldown time in seconds
    private float _timer;
    private float _cooldownTimer = 0f; // Timer to track cooldown
    private Vector2 _direction = Vector2.right; // Start moving to the right by default
    public EquationGenerator equationGenerator;
    public MathManager gameManager;

    public GameObject bodyPrefab; // Prefab for the snake's body part
    public List<Transform> bodyParts = new List<Transform>();
    private List<Vector2> bodyPartDirections = new List<Vector2>(); // Store the direction of each body part
    public int initialBodyParts = 5;

    public Sprite headRightSprite; // Sprite for the head looking right (default)
    public Sprite headLeftSprite;  // Sprite for the head looking left

    private SpriteRenderer spriteRenderer;
    private float initialZPosition;

    // Define the borders of the rectangle
    private const float minX = -7f;
    private const float maxX = 7f;
    private const float minY = -3f;
    private const float maxY = 4f;

    public GameObject deathEffect; // The object to activate upon death
    public AudioSource deathAudioSource; // AudioSource to play the death sound

    public AudioSource audioSource;

    public AudioSource kalhspera;

    void Start()
    {
        initialZPosition = transform.position.z;
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Set the initial sprite to the right-facing sprite
        spriteRenderer.sprite = headRightSprite;

        for (int i = 0; i < initialBodyParts; i++)
        {
            AddBodyPart();
        }
    }

    void Update()
    {
        // Reduce the cooldown timer
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }

        // Check for direction input and update direction & sprite accordingly if cooldown has expired
        if (_cooldownTimer <= 0f)
        {
            bool canMoveUp = transform.position.y < maxY;
            bool canMoveDown = transform.position.y > minY;
            bool canMoveLeft = transform.position.x > minX;
            bool canMoveRight = transform.position.x < maxX;

            if (Input.GetKeyDown(KeyCode.UpArrow) && _direction != Vector2.down && canMoveUp)
            {
                _direction = Vector2.up;
                spriteRenderer.sprite = headRightSprite; // Use the right sprite but flip vertically
                spriteRenderer.flipY = true; // Flip the sprite vertically for upward movement
                _cooldownTimer = directionChangeCooldown; // Reset the cooldown timer
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && _direction != Vector2.up && canMoveDown)
            {
                _direction = Vector2.down;
                spriteRenderer.sprite = headRightSprite; // Use the right sprite but flip vertically
                spriteRenderer.flipY = false; // No flip for downward movement
                _cooldownTimer = directionChangeCooldown; // Reset the cooldown timer
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && _direction != Vector2.right && canMoveLeft)
            {
                _direction = Vector2.left;
                spriteRenderer.sprite = headLeftSprite; // Switch to left-facing sprite
                spriteRenderer.flipY = false; // Ensure no vertical flip when moving left
                _cooldownTimer = directionChangeCooldown; // Reset the cooldown timer
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && _direction != Vector2.left && canMoveRight)
            {
                _direction = Vector2.right;
                spriteRenderer.sprite = headRightSprite; // Switch back to right-facing sprite
                spriteRenderer.flipY = false; // Ensure no vertical flip when moving right
                _cooldownTimer = directionChangeCooldown; // Reset the cooldown timer
            }
        }
    }

    void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer >= moveSpeed)
        {
            _timer = 0f;
            Move();
        }
    }

    void Move()
    {
        Vector2 previousPosition = transform.position;

        // Calculate the potential new position
        Vector3 newPosition = new Vector3(
            Mathf.Round(transform.position.x) + _direction.x,
            Mathf.Round(transform.position.y) + _direction.y,
            initialZPosition
        );

        // Check if the new position is within the bounds
        if (newPosition.x >= minX && newPosition.x <= maxX && newPosition.y >= minY && newPosition.y <= maxY)
        {
            // Move the head to the new position if within bounds
            transform.position = newPosition;
        }
        else
        {
            // If out of bounds, stop further movement in that direction
            return;
        }

        // Rotate the head to face the direction of movement
        if (_direction == Vector2.up || _direction == Vector2.down)
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            transform.rotation = Quaternion.identity; // Reset rotation when moving left or right
        }

        // Move body parts and rotate them based on their own direction
        for (int i = 0; i < bodyParts.Count; i++)
        {
            Vector2 tempPosition = bodyParts[i].position;
            Vector2 tempDirection = bodyPartDirections[i]; // Get the direction of the current body part

            bodyParts[i].position = new Vector3(previousPosition.x, previousPosition.y, initialZPosition);

            // Rotate body part based on its own direction
            if (tempDirection == Vector2.up || tempDirection == Vector2.down)
            {
                bodyParts[i].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
            else
            {
                bodyParts[i].rotation = Quaternion.identity; // Default rotation for left/right movement
            }

            previousPosition = tempPosition;
            bodyPartDirections[i] = _direction; // Update the direction for the current body part
        }

        bodyPartDirections.Insert(0, _direction); // Insert the head's direction at the start
        if (bodyPartDirections.Count > bodyParts.Count)
        {
            bodyPartDirections.RemoveAt(bodyPartDirections.Count - 1); // Keep the list size in sync with body parts
        }
    }

    void AddBodyPart()
    {
        GameObject bodyPart = Instantiate(bodyPrefab);
        bodyPart.transform.position = new Vector3(
            bodyParts.Count == 0 ? transform.position.x : bodyParts[bodyParts.Count - 1].position.x,
            bodyParts.Count == 0 ? transform.position.y : bodyParts[bodyParts.Count - 1].position.y,
            initialZPosition
        );

        bodyParts.Add(bodyPart.transform);
        bodyPartDirections.Add(_direction); // Initialize the direction of the new body part

        bodyPart.tag = "Untagged"; // Temporarily set the tag to "Untagged"
        StartCoroutine(SetTagAfterDelay(bodyPart, "SnakeBody", 1f)); // Set the tag to "SnakeBody" after 1 second
    }

    IEnumerator SetTagAfterDelay(GameObject bodyPart, string newTag, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (kalhspera != null){
                kalhspera.Play(); // Play the attached sound effect
        }
        bodyPart.tag = newTag;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called with: " + other.name);

        if (other.CompareTag("Circle"))
        {
            TextMesh numberText = other.GetComponentInChildren<TextMesh>();
            if (numberText != null)
            {
                int number;
                if (int.TryParse(numberText.text, out number))
                {
                    if (number == equationGenerator.GetCorrectAnswer())
                    {   
                        
                        Debug.Log("Correct number! Proceeding to next round.");
                        kalhspera.Stop();
                        StartCoroutine(HandleRightSequence()); // Move to the next level
                    }
                    else
                    {
                        Debug.Log("Incorrect number! Game over.");
                        kalhspera.Stop();
                        StartCoroutine(HandleDeathSequence()); // Handle death sequence
                    }
                }
                else
                {
                    Debug.LogError("Failed to parse number from circle text.");
                }
            }
            else
            {
                Debug.LogError("TextMesh component not found in collided circle.");
            }
        }
        
        // Check for collision with the snake's own body parts
        if (other.CompareTag("SnakeBody"))
        {
            Debug.Log("Snake collided with its own body! Game over.");
            StartCoroutine(HandleDeathSequence()); // Handle death sequence
        }
    }

    IEnumerator HandleDeathSequence()
    {
        // Activate the death effect object
        if (deathEffect != null)
        {
            // Play the death sound effect
            if (deathAudioSource != null)
            {
                deathAudioSource.Play(); // Play the attached sound effect
            }

            // Activate the visual effect
            deathEffect.SetActive(true);
        }

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Trigger game over
        gameManager.GameOver();
    }

    IEnumerator HandleRightSequence()
    {
        // Activate the death effect object
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Wait for 1 second
        yield return new WaitForSeconds(0.45f);

        // Trigger game over
        gameManager.LoadNextLevel(); // Move to the next level
    }
}


