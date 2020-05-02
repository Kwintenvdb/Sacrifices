using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource drumRollAudio;

    [SerializeField] private AudioSource manScream;
    [SerializeField] private AudioSource womanScream;
    [SerializeField] private AudioSource wilhelmScream;
    [SerializeField] private AudioSource splash;
    [SerializeField] private AudioSource volcanoNoise;
    [SerializeField] private AudioSource impactSound;

    private void Awake()
    {
        Game.Instance.SacrificeDraggingStarted += OnDraggingStarted;
        Game.Instance.SacrificeThrown += OnSacrificeThrown;
        Game.Instance.SacrificeKilled += OnSacrificeKilled;
        Game.Instance.SacrificeReleased += OnSacrificeReleased;
        Game.Instance.SacrificeHitTerrain += OnTerrainHit;
    }

    private void OnTerrainHit(Sacrifice sacrifice)
    {
        impactSound.Play();
    }

    private void OnSacrificeKilled(Sacrifice sacrifice)
    {
        volcanoNoise.Play();
    }

    private void OnSacrificeReleased(Sacrifice sacrifice)
    {
        splash.Play();
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
