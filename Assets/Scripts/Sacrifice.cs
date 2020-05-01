using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

//public enum MovementState
//{
//    Idle,
//    BeingDragged,
//    Flying
//}

public class Sacrifice : MonoBehaviour
{
    [SerializeField] private string sacrificeName;
    [SerializeField] private string description;
    [SerializeField] private float godFavorModifier;
    [SerializeField] private float peopleFavorModifier;
    [SerializeField] public Transform queueSpot;
    [SerializeField] public float speed = 10;

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

    private void Update()
    {
        if (transform.position.y < -25.0)
        {
            Game.Instance.RaiseSacrificeMissed(this);
            print("Sacrifice missed");
            Destroy(gameObject);
        }
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
