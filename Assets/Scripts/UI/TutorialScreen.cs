using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScreen : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("Return")))
        {
            StartGame();
        }
    }
}
