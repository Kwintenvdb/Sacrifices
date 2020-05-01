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
    [TextArea] public string description;
    public SacrificeType type;
}

public class Sacrifice : MonoBehaviour
{
    [SerializeField] private SacrificeData sacrificeData;
    [SerializeField] private string sacrificeName;
    [SerializeField] private float kingFavorNegativeModifier;
    [SerializeField] public Transform queueSpot;
    [SerializeField] public float speed = 10;
    [SerializeField] public Rigidbody rigidBodyToBeDragged;
    [SerializeField] public Collider eventCollider;
    [SerializeField] private ParticleSystem splashParticles;
    [SerializeField] private ParticleSystem fireballParticles;

    public SacrificeData Data => sacrificeData;
    public float KingFavorNegativeModifier => kingFavorNegativeModifier;
    public bool IsReady { get; private set; }
    public MovementState MovementState { get; set; }
    public bool IsFlying => MovementState == MovementState.Flying;
    public bool IsOnFloor => MovementState == MovementState.CollidedWithTerrain;

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
        if (this != null) // trust me, we need this check
        {
            print("Sacrifice going to miss");
            Game.Instance.RaiseSacrificeMissed(this);
            print("Sacrifice missed");
            Destroy(gameObject);
        }
    }

    public void Kill()
    {
        if (this != null)
        {
            print("Sacrifice going to be killed");
            Game.Instance.RaiseSacrificeHappened(this);
            print("Sacrifice killed");
            Instantiate(fireballParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Release()
    {
        if (this != null)
        {
            print("Sacrifice going to be released");
            Game.Instance.RaiseSacrificeReleased(this);
            Instantiate(splashParticles, transform.position, Quaternion.identity);
            print("Sacrifice released");

            // TODO: play some sort of animation instead...
            Destroy(gameObject);
        }
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
    }

    public void AllowCollision(bool allow)
    {
        eventCollider.isTrigger = !allow;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (MovementState != MovementState.CollidedWithTerrain)
        {
            MovementState = MovementState.CollidedWithTerrain;
            StartCoroutine(ResetAfterDelay());
        }
    }

    private IEnumerator ResetAfterDelay()
    {
        // Allow 2 seconds of lying on the floor before resetting.
        yield return new WaitForSeconds(2);

        // All this code is ugly as shit but I don't want to change it.
        transform.position = queueSpot.position;
        transform.rotation = Quaternion.identity;
        GetComponentInChildren<Animator>().enabled = true;
        foreach (Rigidbody rigidbody in GetComponentsInChildren<Rigidbody>())
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }
        MovementState = MovementState.Idle;
        AllowCollision(false);
        RaiseReadyIfFirstInQueue();
    }
}
