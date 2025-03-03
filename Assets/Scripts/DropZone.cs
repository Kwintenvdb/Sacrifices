﻿using UnityEngine;

// TODO: Need extra "Miss" drop zone that we can put in other places in the level
public enum DropZoneType
{
    Kill,
    Release,
    Miss
}

public class DropZone : MonoBehaviour
{
    [SerializeField] private DropZoneType dropZoneType;

    private void OnTriggerEnter(Collider other)
    {
        var sacrifice = other.GetComponent<Sacrifice>();
        if (sacrifice && (sacrifice.IsFlying || sacrifice.IsOnFloor))
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
        else if (dropZoneType == DropZoneType.Miss)
        {
            sacrifice.Miss();
        }
        else
        {
            sacrifice.Release();
        }
    }
}
