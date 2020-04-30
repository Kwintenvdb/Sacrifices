using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MovementState
{
    Idle,
    BeingDragged,
    Flying
}

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
    [SerializeField] private int queuePosition;

    private MovementState movementState = MovementState.Idle;

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
        // some queue movement code in here maybe
    }

}
