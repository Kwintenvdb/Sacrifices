using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public static Queue Instance { get; private set; }

    [SerializeField] private List<Transform> queueSpots;
    private Dictionary<Transform, Sacrifice> assignments = new Dictionary<Transform, Sacrifice>();

    public bool isMoving = false;

    public bool IsEmpty => assignments.Count == 0;

    private void Awake()
    {
        Instance = this;

        queueSpots = new List<Transform>();
        foreach (Transform t in transform)
        {
            queueSpots.Add(t);
        }

        Game.Instance.SacrificeKilled += Dequeue;
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

    private void Dequeue(Sacrifice sacrifice)
    {
        assignments.Remove(sacrifice.queueSpot);
        sacrifice.queueSpot = null;

        for (int i = 0; i < queueSpots.Count - 1; ++i)
        {
            if (assignments.ContainsKey(queueSpots[i + 1]))
            {
                assignments[queueSpots[i]] = assignments[queueSpots[i + 1]];
                assignments[queueSpots[i + 1]].queueSpot = queueSpots[i];
                assignments.Remove(queueSpots[i + 1]);
            }
        }

        StartCoroutine(MoveSacrificesOneByOne());
    }

    private IEnumerator MoveSacrificesOneByOne()
    {
        isMoving = true;
        // A small delay to let the king have a comment on what happened before
        yield return new WaitForSeconds(2);

        foreach (var assignment in assignments)
        {
            var sacrifice = assignment.Value;
            sacrifice.MoveTowardsQueueSpot();
            yield return new WaitForSeconds(0.2f);
        }
        isMoving = false;
    }
}
