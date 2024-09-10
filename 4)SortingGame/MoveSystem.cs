using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    private AudioSource audioSource;
    private GameObject correctForm;
    private bool moving;
    private bool finish;

    private float startPosX;
    private float startPosY;

    private Vector3 resetPosition;

    private ShapeShadowManager manager;

    void Start()
    {   
        audioSource = GetComponent<AudioSource>();
        resetPosition = this.transform.localPosition;
        if(!PlayerPrefs.HasKey("unlock5")){
            PlayerPrefs.SetInt("unlock5", 0);
            PlayerPrefs.Save();
        }

    }

    void Update()
    {  
        if (!finish && moving)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;

            moving = true;
        }
    }

    private void OnMouseUp()
    {
        moving = false;

        if (Mathf.Abs(this.transform.localPosition.x - correctForm.transform.localPosition.x) <= 0.5f && Mathf.Abs(this.transform.localPosition.y - correctForm.transform.localPosition.y) <= 0.5f)
        {
            this.transform.position = new Vector3(correctForm.transform.position.x, correctForm.transform.position.y, this.gameObject.transform.localPosition.z);
            finish = true;
            audioSource.Play();
            Debug.Log("Object tapped!");
            // Notify the manager that this piece is placed correctly
            manager.IncrementCounter();
        }
        else
        {
            this.transform.localPosition = resetPosition;
        }
    }

    public void Initialize(GameObject shadow)
    {
        correctForm = shadow;
        manager = FindObjectOfType<ShapeShadowManager>();
    }
}