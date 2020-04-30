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

public class DraggingController : MonoBehaviour, IDragHandler, IDropHandler, IPointerUpHandler
{
    [SerializeField] private Transform draggingCenterPoint;
    [SerializeField] private float draggingRadius;
    [SerializeField] private new Rigidbody rigidbody;

    private MovementState movementState = MovementState.Idle;

    public void OnDrag(PointerEventData eventData)
    {
        movementState = MovementState.BeingDragged;

        var targetPos = eventData.pointerCurrentRaycast.worldPosition;
        targetPos.z = 0;

        var centerPos = draggingCenterPoint.position;
        var distance = Vector3.Distance(targetPos, centerPos);
        if (distance > draggingRadius)
        {
            var directionCenterToTarget = (targetPos - centerPos).normalized;
            targetPos = centerPos + directionCenterToTarget * draggingRadius;
        }

        transform.position = targetPos;
    }

    public void OnDrop(PointerEventData eventData)
    {
        print("drop");
        if (movementState == MovementState.BeingDragged)
        {
            movementState = MovementState.Flying;
            rigidbody.useGravity = true;
            rigidbody.AddExplosionForce(1000, draggingCenterPoint.position, 10);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("up");
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(draggingCenterPoint.position, Vector3.forward,
            draggingRadius);
    }
#endif
}
