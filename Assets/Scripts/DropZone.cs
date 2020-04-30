using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropZoneType
{
    Kill,
    Release
}

public class DropZone : MonoBehaviour
{
    [SerializeField] private DropZoneType dropZoneType;

    [SerializeField] private Sacrifice readySacrifice;


    private void Awake()
    {
        Game.Instance.SacrificeReady += SetReadySacrifice;
    }

    void SetReadySacrifice(Sacrifice sacrifice)
    {
        readySacrifice = sacrifice;
    }

    private void OnTriggerEnter(Collider other)
    {
        var sacrifice = other.GetComponent<Sacrifice>();
        if (sacrifice && sacrifice == readySacrifice)
        {
            KillOrReleaseSacrifice(sacrifice);
        }
    }

    private void KillOrReleaseSacrifice(Sacrifice sacrifice)
    {
        if (dropZoneType == DropZoneType.Kill)
        {
            sacrifice.Kill();
        }
        else
        {
            sacrifice.Release();
        }
    }
}
