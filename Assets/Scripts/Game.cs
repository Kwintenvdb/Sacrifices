using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private readonly List<SacrificeData> peopleSacrificed = new List<SacrificeData>();
    private readonly List<SacrificeData> peopleSaved = new List<SacrificeData>();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this); // Won't be destroyed when transitioning to end game screen.
    }

    public void RaiseSacrificeHappened(Sacrifice sacrifice)
    {
        peopleSacrificed.Add(sacrifice.Data);
        SacrificeKilled?.Invoke(sacrifice);
        CheckGameEnd();
    }

    public void RaiseSacrificeReleased(Sacrifice sacrifice)
    {
        peopleSaved.Add(sacrifice.Data);
        SacrificeReleased?.Invoke(sacrifice);
        CheckGameEnd();
    }

    public void RaiseSacrificeMissed(Sacrifice sacrifice)
    {
        SacrificeReleased?.Invoke(sacrifice);
        CheckGameEnd();
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

    private void CheckGameEnd()
    {
        if (Queue.Instance.IsEmpty)
        {
            SceneManager.LoadScene(1);
        }
    }
}
