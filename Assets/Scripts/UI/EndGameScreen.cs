using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] private Text sacrificeNameText;
    [SerializeField] private Text sacrificeFlavorText;

    private List<SacrificeResult> results = new List<SacrificeResult>();

    // TODO
    // Fetch information from Game instance
    // Display information in scene / spawn objects as needed
    void Awake()
    {
        // TODO: What to show if lost?

        Cursor.visible = true;

        // For debugging only
        if (!Game.Instance) return;

        results = Game.Instance.Sacrifices;
        foreach (SacrificeResult sacrificeResult in results)
        {
            GetComponentInChildren<Text>().text += "\n" + sacrificeResult.sacrifice.name + " was " + (sacrificeResult.type == DropZoneType.Kill? "killed. " + sacrificeResult.sacrifice.killedDescription : "released. " + sacrificeResult.sacrifice.releasedDescription);
        }
    }

    public void RestartGame()
    {
        Destroy(Game.Instance.gameObject);
        SceneManager.LoadScene("TutorialScene");
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var sacrifice = hit.collider.GetComponent<Sacrifice>();
            if (sacrifice)
            {
                var result = results.Find(r => r.sacrifice.name == sacrifice.Data.name);
                ShowSacrificeResult(result);
                return;
            }
        }

        HideSacrificeResult();
    }

    private void ShowSacrificeResult(SacrificeResult result)
    {
        sacrificeNameText.text = result.sacrifice.name;
        var flavorText = result.type == DropZoneType.Kill ? result.sacrifice.killedDescription : result.sacrifice.releasedDescription;
        sacrificeFlavorText.text = flavorText;
    }

    private void HideSacrificeResult()
    {
        sacrificeNameText.text = null;
        sacrificeFlavorText.text = null;
    }
}
