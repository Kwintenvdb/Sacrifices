using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource drumRollAudio;

    private void Awake()
    {
        Game.Instance.SacrificeDraggingStarted += OnDraggingStarted;
    }

    private void OnDraggingStarted(Sacrifice sacrifice)
    {
        drumRollAudio.Play();
    }
}
