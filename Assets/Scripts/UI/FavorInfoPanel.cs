using UnityEngine;
using UnityEngine.UI;

public class FavorInfoPanel : MonoBehaviour
{
    [SerializeField] private Image godFavorFill;
    [SerializeField] private Image peopleFavorFill;

    private void Awake()
    {
        Game.Instance.PointsUpdated += OnPointsUpdated;
    }

    private void OnPointsUpdated(float godFavor, float peopleFavor)
    {
        godFavorFill.fillAmount = godFavor / 100;
        peopleFavorFill.fillAmount = peopleFavor / 100;
    }
}
