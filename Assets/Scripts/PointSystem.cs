using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [SerializeField] [Range(0.0F, 1.0F)] private float missPenalty;
    [SerializeField] private float kingFavor;
    //[SerializeField] private float peopleFavor;

    public float KingFavor => kingFavor;

    void Awake()
    {
        Game.Instance.SacrificeReleased += SacrificeReleased;
        Game.Instance.SacrificeMissed += SacrificeMissed;
    }

    public void SacrificeReleased(Sacrifice sacrifice)
    {
        kingFavor -= sacrifice.KingFavorNegativeModifier;
        RaisePointsChangedEvent();
    }

    public void SacrificeMissed(Sacrifice sacrifice)
    {
        // TODO: Do anything?
    }

    private void RaisePointsChangedEvent()
    {
        Game.Instance.RaisePointsUpdated(kingFavor);
    }
}
