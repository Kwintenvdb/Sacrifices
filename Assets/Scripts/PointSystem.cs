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
        RaisePointsChangedEvent();
        CheckGodFavor();
    }

    public void SacrificeReleased(Sacrifice sacrifice)
    {
        peopleFavor += sacrifice.PeopleFavorModifier;
        RaisePointsChangedEvent();
        CheckPeopleFavor();
    }

    public void SecrificeMissed(Sacrifice sacrifice)
    {
        peopleFavor = sacrifice.PeopleFavorModifier > 0 ? sacrifice.PeopleFavorModifier * missPenalty : sacrifice.PeopleFavorModifier * (1.0f + missPenalty);
        godFavor = sacrifice.GodFavorModifier > 0 ? sacrifice.GodFavorModifier * missPenalty : sacrifice.GodFavorModifier * (1.0f + missPenalty);
        RaisePointsChangedEvent();
        CheckPeopleFavor();
        CheckGodFavor();
    }

    private void RaisePointsChangedEvent()
    {
        Game.Instance.RaisePointsUpdated(godFavor, peopleFavor);
    }

    private void CheckGodFavor()
    {
        if (godFavor >= 100.0)
        {
            print("THE GODS ACKNOWLEDGE YOU!");
        } else if (godFavor <= 0.0)
        {
            print("THE GODS CONDEMN YOU!");
        }
    }

    private void CheckPeopleFavor()
    {
        if (godFavor >= 100.0)
        {
            print("THE PEOPLE ACKNOWLEDGE YOU!");
        }
        else if (godFavor <= 0.0)
        {
            print("THE PEOPLE CONDEMN YOU!");
        }
    }
}
