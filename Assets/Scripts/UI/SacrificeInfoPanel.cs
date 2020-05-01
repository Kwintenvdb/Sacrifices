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
    }

    // Hide panel while sacrifice is being thrown, show again when next is ready.
    private void OnSacrificeThrown(Sacrifice sacrifice)
    {
        gameObject.SetActive(false);
    }

    private void OnSacrificeReady(Sacrifice sacrifice)
    {
        gameObject.SetActive(true);

        // Update UI info
        //nameText.text = sacrifice.Name;
        descriptionText.text = sacrifice.Description;
    }
}
