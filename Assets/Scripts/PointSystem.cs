using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [SerializeField] [Range(0.0F, 1.0F)] private float missPenalty;
    [SerializeField] private float godFavor;
    [SerializeField] private float peopleFavor;

    void Awake()
    {
        Game.Instance.SacrificeKilled += GodSacrifice;
        Game.Instance.SacrificeReleased += SacrificeReleased;
        Game.Instance.SacrificeMissed += SecrificeMissed;
    }
    public void GodSacrifice(Sacrifice sacrifice)
    {
        godFavor += sacrifice.GodFavorModifier;
        CheckGodFavor();

    }

    public void SacrificeReleased(Sacrifice sacrifice)
    {
        peopleFavor += sacrifice.PeopleFavorModifier;
        CheckPeopleFavor();
    }

    public void SecrificeMissed(Sacrifice sacrifice)
    {
        peopleFavor = sacrifice.PeopleFavorModifier > 0 ? sacrifice.PeopleFavorModifier * missPenalty : sacrifice.PeopleFavorModifier * (1.0f + missPenalty);
        godFavor = sacrifice.GodFavorModifier > 0 ? sacrifice.GodFavorModifier * missPenalty : sacrifice.GodFavorModifier * (1.0f + missPenalty);
        CheckPeopleFavor();
        CheckGodFavor();
    }

    private void CheckGodFavor()
    {
        // TODO
    }

    private void CheckPeopleFavor()
    {
        // TODO
    }
}
