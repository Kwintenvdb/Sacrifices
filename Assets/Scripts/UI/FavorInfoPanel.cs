using UnityEngine;
using UnityEngine.UI;

public class FavorInfoPanel : MonoBehaviour
{
    [SerializeField] private Image kingFavorFill;
    [SerializeField] private PointSystem pointSystem;

    private void Awake()
    {
        Game.Instance.PointsUpdated += OnPointsUpdated;
        OnPointsUpdated(pointSystem.KingFavor);
    }

    private void OnPointsUpdated(float kingFavor)
    {
        kingFavorFill.fillAmount = kingFavor / 100;
    }
}
