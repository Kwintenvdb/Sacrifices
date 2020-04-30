using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public static Queue Instance { get; private set; }

    [SerializeField] private List<Transform> queueSpots;
    private Dictionary<Transform, Sacrifice> assignments = new Dictionary<Transform, Sacrifice>();

    private void Awake()
    {
        Instance = this;

        queueSpots = new List<Transform>();
        foreach (Transform t in transform)
        {
            queueSpots.Add(t);
        }

        Game.Instance.SacrificeKilled += Dequeue;
        Game.Instance.SacrificeMissed += Dequeue;
        Game.Instance.SacrificeReleased += Dequeue;
    }


    public void Enqueue(Sacrifice sacrifice)
    {
        foreach (Transform t in queueSpots)
        {
            if (!assignments.ContainsKey(t))
            {
                assignments.Add(t, sacrifice);
                sacrifice.queueSpot = t;
                return;
            }
        }
    }

    void Dequeue(Sacrifice sacrifice)
    {
        assignments.Remove(sacrifice.queueSpot);
        sacrifice.queueSpot = null;

        for (int i = 0; i < queueSpots.Count - 1; ++i)
        {
            if (assignments.ContainsKey(queueSpots[i + 1]))
            {
                assignments[queueSpots[i]] =  assignments[queueSpots[i + 1]];
                assignments[queueSpots[i + 1]].queueSpot = queueSpots[i];
                assignments.Remove(queueSpots[i + 1]);
            }
        }
    }
}
