using System;
using UnityEngine;

public delegate void PointsUpdated(float godFavor, float peopleFavor);

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public event Action<Sacrifice> SacrificeKilled;
    public event Action<Sacrifice> SacrificeReleased;
    public event Action<Sacrifice> SacrificeMissed;

    public event Action<Sacrifice> SacrificeReady;
    public event Action<Sacrifice> SacrificeDraggingStarted;
    public event Action<Sacrifice> SacrificeThrown;

    public event PointsUpdated PointsUpdated;

    private void Awake()
    {
        Instance = this;
    }

    public void RaiseSacrificeHappened(Sacrifice sacrifice)
    {
        SacrificeKilled?.Invoke(sacrifice);
    }

    public void RaiseSacrificeReleased(Sacrifice sacrifice)
    {
        SacrificeReleased?.Invoke(sacrifice);
    }

    public void RaiseSacrificeMissed(Sacrifice sacrifice)
    {
        SacrificeReleased?.Invoke(sacrifice);
    }

    public void RaiseSacrificeReady(Sacrifice sacrifice)
    {
        SacrificeReady?.Invoke(sacrifice);
    }

    public void RaiseSacrificeDraggingStarted(Sacrifice sacrifice)
    {
        SacrificeDraggingStarted?.Invoke(sacrifice);
    }

    public void RaiseSacrificeThrown(Sacrifice sacrifice)
    {
        SacrificeThrown?.Invoke(sacrifice);
    }

    public void RaisePointsUpdated(float godFavor, float peopleFavor)
    {
        PointsUpdated?.Invoke(godFavor, peopleFavor);
    }
}
