using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void PointsUpdated(float kingFavor);

public struct SacrificeResult
{
    public SacrificeData sacrifice;
    public DropZoneType type;

    public SacrificeResult(SacrificeData sacrifice, DropZoneType type) : this()
    {
        this.sacrifice = sacrifice;
        this.type = type;
    }
}

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public event Action<Sacrifice> SacrificeKilled;
    public event Action<Sacrifice> SacrificeReleased;
    public event Action<Sacrifice> SacrificeMissed;
    public event Action<Sacrifice> SacrificeHitTerrain;

    public event Action<Sacrifice> SacrificeReady;
    public event Action<Sacrifice> SacrificeDraggingStarted;
    public event Action<Sacrifice> SacrificeThrown;

    public event PointsUpdated PointsUpdated;


    public List<SacrificeResult> Sacrifices = new List<SacrificeResult>();

    private readonly List<SacrificeData> peopleSacrificed = new List<SacrificeData>();
    private readonly List<SacrificeData> peopleSaved = new List<SacrificeData>();


    public bool Lost { get; private set; }

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
        SacrificeMissed?.Invoke(sacrifice);
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

    public void RaisePointsUpdated(float kingFavor)
    {
        PointsUpdated?.Invoke(kingFavor);
        if (kingFavor <= 0)
        {
            print("lost");
            // Lose condition
            Lost = true;
            LoadEndGameScene();
        }
    }

    public void RaiseSacrificeHitTerrain(Sacrifice sacrifice)
    {
        SacrificeHitTerrain?.Invoke(sacrifice);
    }

    private void CheckGameEnd()
    {
        if (Lost) return; // We're already loading the end game scene.
        if (Queue.Instance.IsEmpty)
        {
            LoadEndGameScene();
        }
    }

    private void LoadEndGameScene()
    {
        StartCoroutine(LoadEndGameSceneDelayed());
    }

    private IEnumerator LoadEndGameSceneDelayed()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
}
