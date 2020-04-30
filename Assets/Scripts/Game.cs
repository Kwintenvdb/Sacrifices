using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public event Action<Sacrifice> SacrificeKilled;
    public event Action<Sacrifice> SacrificeReleased;
    // TODO Sacrifice "missed" event

    public event Action<Sacrifice> SacrificeReady;

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

    public void RaiseSacrificeReady(Sacrifice sacrifice)
    {
        SacrificeReady?.Invoke(sacrifice);
    }
}
