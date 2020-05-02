#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;

public enum MovementState
{
    Idle,
    BeingDragged,
    CollidedWithTerrain,
    Flying
}

public class DraggingController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform draggingCenterPoint;
    [SerializeField] private float draggingRadius;
    [SerializeField] private SpringJoint dragJoint;
    [SerializeField] private float rigidbodyDragWhileDragging = 1.5f;
    [SerializeField] private Transform hand;

    private Sacrifice sacrifice;
    private Rigidbody sacrificeRigidbody;

    protected void Awake()
    {
        Game.Instance.SacrificeReady += OnSacrificeReady;
        Cursor.visible = false;
    }

    protected void Update()
    {
        MoveHand();
    }

    private void MoveHand()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var worldPos = hit.point;
            worldPos.z = 0;
            hand.position = RestrictToCircle(worldPos);
        }
    }

    private void OnSacrificeReady(Sacrifice sacrifice)
    {
        this.sacrifice = sacrifice;
        dragJoint.connectedBody = sacrifice.getRigidbodyToBeDragged();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!sacrifice || !sacrifice.IsReady || sacrifice.IsFlying || Queue.Instance.isMoving) return;

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
        targetPos = RestrictToCircle(targetPos);
        dragJoint.transform.position = targetPos;
    }

    private Vector3 RestrictToCircle(Vector3 pos)
    {
        var centerPos = draggingCenterPoint.position;
        var distance = Vector3.Distance(pos, centerPos);
        if (distance > draggingRadius)
        {
            var directionCenterToTarget = (pos - centerPos).normalized;
            pos = centerPos + directionCenterToTarget * draggingRadius;
        }
        return pos;
    }

    private void EnableSacrificeRigidbodyDragging()
    {
        sacrifice.AllowCollision(false);
        sacrifice.UseGravity(rigidbodyDragWhileDragging);
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
        sacrifice.UseGravity(0);
        sacrifice.AllowCollision(true);
        dragJoint.connectedBody = null;
        sacrifice = null;
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
