using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    // TODO
    // Fetch information from Game instance
    // Display information in scene / spawn objects as needed
    void Awake()
    {
        Cursor.visible = true;
        foreach (SacrificeResult sacrificeResult in Game.Instance.Sacrifices)
        {
            GetComponentInChildren<Text>().text += "\n" + sacrificeResult.sacrifice.name + " was " + (sacrificeResult.type == DropZoneType.Kill? "killed." : "released.");
        }
    }

    public void RestartGame()
    {
        Destroy(Game.Instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
