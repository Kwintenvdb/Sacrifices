using UnityEngine;
using UnityEngine.UI;

public class SacrificeInfoPanel : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    // TODO (maybe) "stats" about sacrifice

    private void Awake()
    {
        Game.Instance.SacrificeReady += OnSacrificeReady;
    }

    private void OnSacrificeReady(Sacrifice sacrifice)
    {
        // Update UI info
        nameText.text = sacrifice.Name;
        descriptionText.text = sacrifice.Description;
    }
}
