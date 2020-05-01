using UnityEngine;
using UnityEngine.UI;

public class FavorInfoPanel : MonoBehaviour
{
    [SerializeField] private Image kingFavorFill;
    [SerializeField] private Image peopleFavorFill;

    private void Awake()
    {
        Game.Instance.PointsUpdated += OnPointsUpdated;
    }

    private void OnPointsUpdated(float kingFavor)
    {
        kingFavorFill.fillAmount = kingFavor / 100;
    }
}
