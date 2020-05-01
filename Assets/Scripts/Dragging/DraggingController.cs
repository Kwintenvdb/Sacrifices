#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;

public enum MovementState
{
    Idle,
    BeingDragged,
    Flying
}

public class DraggingController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform draggingCenterPoint;
    [SerializeField] private float draggingRadius;
    [SerializeField] private float rigidbodyDragWhileDragging = 1.5f;
    [SerializeField] private SpringJoint dragJoint;


    private Sacrifice sacrifice;
    private Rigidbody sacrificeRigidbody;

    protected void Awake()
    {
        Game.Instance.SacrificeReady += OnSacrificeReady;
    }

    private void OnSacrificeReady(Sacrifice sacrifice)
    {
        this.sacrifice = sacrifice;
        sacrificeRigidbody = sacrifice.GetComponent<Rigidbody>();
        dragJoint.connectedBody = sacrificeRigidbody;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!sacrifice || !sacrifice.IsReady || sacrifice.IsFlying) return;

        if (sacrifice.MovementState == MovementState.Idle)
        {
            sacrifice.MovementState = MovementState.BeingDragged;
            EnableSacrificeRigidbodyDragging();
            Game.Instance.RaiseSacrificeDraggingStarted(sacrifice);
        }

        SetDragPointPosition(eventData);
    }

    private void SetDragPointPosition(PointerEventData eventData)
    {
        var targetPos = eventData.pointerCurrentRaycast.worldPosition;
        targetPos.z = 0;
        var centerPos = draggingCenterPoint.position;
        var distance = Vector3.Distance(targetPos, centerPos);
        if (distance > draggingRadius)
        {
            var directionCenterToTarget = (targetPos - centerPos).normalized;
            targetPos = centerPos + directionCenterToTarget * draggingRadius;
        }
        dragJoint.transform.position = targetPos;
    }

    private void EnableSacrificeRigidbodyDragging()
    {
        sacrificeRigidbody.isKinematic = false;
        sacrificeRigidbody.useGravity = true;
        sacrificeRigidbody.drag = rigidbodyDragWhileDragging;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("end drag");
        if (sacrifice && sacrifice.MovementState == MovementState.BeingDragged)
        {
            ThrowSacrifice();
        }
    }

    private void ThrowSacrifice()
    {
        Game.Instance.RaiseSacrificeThrown(sacrifice);
        sacrifice.MovementState = MovementState.Flying;
        sacrificeRigidbody.drag = 0;
        dragJoint.connectedBody = null;
        sacrifice = null;
        sacrificeRigidbody = null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!draggingCenterPoint) return;
        Handles.color = Color.green;
        Handles.DrawWireDisc(draggingCenterPoint.position, Vector3.forward,
            draggingRadius);
    }
#endif
}
