using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScreen : MonoBehaviour
{
    // TODO
    // Fetch information from Game instance
    // Display information in scene / spawn objects as needed

    public void RestartGame()
    {
        Destroy(Game.Instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
