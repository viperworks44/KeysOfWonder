using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishButton : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("FinishAll"); 
    }
}
