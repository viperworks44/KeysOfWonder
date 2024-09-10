using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public Clock clock; // Reference to the Clock script attached to your Clock GameObject

    private void OnMouseDown()
    {
        clock.CheckTime(); // Call the public CheckTime() method of the Clock script
    }
}