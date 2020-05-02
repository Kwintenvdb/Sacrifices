using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SacrificeInfoPanel : MonoBehaviour
{
    //[SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    // TODO (maybe) "stats" about sacrifice

    private void Awake()
    {
        Game.Instance.SacrificeReady += OnSacrificeReady;
        Game.Instance.SacrificeThrown += OnSacrificeThrown;
        Game.Instance.SacrificeKilled += OnSacrificeKilled;
        Game.Instance.SacrificeReleased += OnSacrificeReleased;
    }

    // Hide panel while sacrifice is being thrown, show again when next is ready.
    private void OnSacrificeThrown(Sacrifice sacrifice)
    {
        gameObject.SetActive(false);
    }

    private void ShowKingText(string text)
    {
        gameObject.SetActive(true);
        StartCoroutine(TypeKingText(text));
    }

    private IEnumerator TypeKingText(string text)
    {
        descriptionText.text = null;
        foreach (char c in text)
        {
            descriptionText.text += c;
            yield return new WaitForSeconds(0.015f);
        }
    }

    private void OnSacrificeReady(Sacrifice sacrifice)
    {
        // Update UI info
        ShowKingText(sacrifice.Data.description);
    }
    
    private void OnSacrificeKilled(Sacrifice sacrifice)
    {
        ShowKingText("Another offer to the gods!");
    }

    private void OnSacrificeReleased(Sacrifice sacrifice)
    {
        ShowKingText("Blasphemy!");
    }
}
