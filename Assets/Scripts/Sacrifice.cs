using System;
using System.Collections;
using UnityEditor.Experimental.AssetImporters;
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
    [SerializeField] private float kingFavorNegativeModifier;
    [SerializeField] public Transform queueSpot;
    [SerializeField] public float speed = 10;
    [SerializeField] public Rigidbody rigidBodyToBeDragged;
    [SerializeField] public Collider eventCollider;

    public SacrificeData Data => sacrificeData;
    public string Name => sacrificeName;
    public string Description => description;
    public float KingFavorNegativeModifier => kingFavorNegativeModifier;
    public bool IsReady { get; private set; }
    public MovementState MovementState { get; set; }
    public bool IsFlying => MovementState == MovementState.Flying;

    // Leave in start, makes order of operations easier
    private void Start()
    {
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.isTrigger = true;
        }

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

    public Rigidbody getRigidbodyToBeDragged()
    {
        return rigidBodyToBeDragged;
    }

    public void UseGravity(float drag)
    {
        GetComponentInChildren<Animator>().enabled = false;
        rigidBodyToBeDragged.drag = drag;
        foreach (Rigidbody rigidbody in GetComponentsInChildren<Rigidbody>())
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            //if (collider != eventCollider)
            //{
            //    collider.enabled = false;
            //}
        }
    }

    public void AllowCollision(bool allow)
    {
        eventCollider.isTrigger = !allow;
    }

    public void useKinematic()
    {
        foreach (Rigidbody rigidbody in GetComponentsInChildren<Rigidbody>())
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }
    }
}
