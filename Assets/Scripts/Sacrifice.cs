using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

//public enum MovementState
//{
//    Idle,
//    BeingDragged,
//    Flying
//}

public class Sacrifice : MonoBehaviour, IDragHandler, IPointerUpHandler
{
    // each sacrifice needs some stats:
    // * god favor modifier
    // * people favor modifier
    // * name
    // * flavor text? / description
    // * current position in queue

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

    private MovementState movementState = MovementState.Idle;

    // Leave in start, makes order of operations easier
    private void Start()
    {
        Queue.Instance.Enqueue(this);
        transform.position = queueSpot.position;
        RaiseReadyIfFirstInQueue();
    }

    // handle going into "dragging mode"
    public void OnDrag(PointerEventData eventData)
    {
        // TODO limit dragging area
        movementState = MovementState.BeingDragged;
        var position = eventData.pointerCurrentRaycast.worldPosition;
        position.z = 0;
        transform.position = position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Later: actually start flying with velocity of drag
        if (movementState == MovementState.BeingDragged)
        {
            movementState = MovementState.Flying;
        }
        // TODO start detecting collision with sacrifice / release zones
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

    private void Update()
    {
        //if (movementState == MovementState.Idle)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, queueSpot.position, Time.deltaTime * speed);
        //    if (transform.position == queueSpot.position && queueSpot.gameObject.tag == "READY_SPOT")
        //    {
        //        Game.Instance.RaiseSacrificeReady(this);
        //    }
        //}
    }

    public void MoveTowardsQueueSpot()
    {
        StartCoroutine(MovementCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        print("start moving");
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
        }
    }
}
