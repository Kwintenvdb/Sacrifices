using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [SerializeField] [Range(1.0F, 2.0F)] private float missPenalty = 1.15f;
    [SerializeField] private float kingFavor = 100;
    //[SerializeField] private float peopleFavor;

    public float KingFavor => kingFavor;

    void Awake()
    {
        Game.Instance.SacrificeReleased += SacrificeReleased;
        Game.Instance.SacrificeMissed += SacrificeMissed;
        Game.Instance.SacrificeHitTerrain += SacrificeMissed;
    }

    public void SacrificeReleased(Sacrifice sacrifice)
    {
        kingFavor -= sacrifice.KingFavorNegativeModifier;
        RaisePointsChangedEvent();
    }

    public void SacrificeMissed(Sacrifice sacrifice)
    {
        sacrifice.kingFavorNegativeModifier *= missPenalty;
    }

    private void RaisePointsChangedEvent()
    {
        Game.Instance.RaisePointsUpdated(kingFavor);
    }
}
