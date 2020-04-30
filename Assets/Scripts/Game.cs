using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public event Action<Sacrifice> SacrificeHappened;

    private void Awake()
    {
        Instance = this;
    }

    public void RaiseSacrificeHappened(Sacrifice sacrifice)
    {
        SacrificeHappened?.Invoke(sacrifice);
    }
}
