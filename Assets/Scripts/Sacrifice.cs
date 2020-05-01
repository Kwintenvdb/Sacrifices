using System;
using System.Collections;
using UnityEngine;

public enum SacrificeType
{
    Male,
    Female,
    Wilhelm
}

[Serializable]
public struct SacrificeData
{
    public string name;
    public string description;
    public SacrificeType type;
}

public class Sacrifice : MonoBehaviour
{
    [SerializeField] private SacrificeData sacrificeData;
    [SerializeField] private string sacrificeName;
    [TextArea] [SerializeField] private string description;
    [SerializeField] private float godFavorModifier;
    [SerializeField] private float peopleFavorModifier;
    [SerializeField] public Transform queueSpot;
    [SerializeField] public float speed = 10;

    public SacrificeData Data => sacrificeData;
    public string Name => sacrificeName;
    public string Description => description;
    public float GodFavorModifier => godFavorModifier;
    public float PeopleFavorModifier => peopleFavorModifier;
    public bool IsReady { get; private set; }
    public MovementState MovementState { get; set; }
    public bool IsFlying => MovementState == MovementState.Flying;

    // Leave in start, makes order of operations easier
    private void Start()
    {
        Queue.Instance.Enqueue(this);
        transform.position = queueSpot.position;
        RaiseReadyIfFirstInQueue();
    }

    public void Miss()
    {
        Game.Instance.RaiseSacrificeMissed(this);
        print("Sacrifice missed");
        Destroy(gameObject);
    }

    public void Kill()
    {
        Game.Instance.RaiseSacrificeHappened(this);
        print("Sacrifice killed");
        Destroy(gameObject);
    }

    public void Release()
    {
        Game.Instance.RaiseSacrificeReleased(this);
        print("Sacrifice released");

        // TODO: play some sort of animation instead...
        Destroy(gameObject);
    }

    public void MoveTowardsQueueSpot()
    {
        StartCoroutine(MovementCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        var targetPos = queueSpot.position;
        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
            yield return null;
        }

        RaiseReadyIfFirstInQueue();
    }

    private void RaiseReadyIfFirstInQueue()
    {
        if (queueSpot.gameObject.tag == "READY_SPOT")
        {
            Game.Instance.RaiseSacrificeReady(this);
            IsReady = true;
        }
    }
}
