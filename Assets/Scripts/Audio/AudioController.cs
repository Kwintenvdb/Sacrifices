using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource drumRollAudio;

    [SerializeField] private AudioSource manScream;
    [SerializeField] private AudioSource womanScream;
    [SerializeField] private AudioSource wilhelmScream;

    private void Awake()
    {
        Game.Instance.SacrificeDraggingStarted += OnDraggingStarted;
        Game.Instance.SacrificeThrown += OnSacrificeThrown;
    }

    private void OnSacrificeThrown(Sacrifice sacrifice)
    {
        switch (sacrifice.Data.type)
        {
            case SacrificeType.Male:
                manScream.Play();
                break;
            case SacrificeType.Female:
                womanScream.Play();
                break;
            case SacrificeType.Wilhelm:
                wilhelmScream.Play();
                break;
        }
    }

    private void OnDraggingStarted(Sacrifice sacrifice)
    {
        drumRollAudio.Play();
    }


}
