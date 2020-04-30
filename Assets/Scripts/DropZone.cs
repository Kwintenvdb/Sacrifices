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

    private void OnTriggerEnter(Collider other)
    {
        var sacrifice = other.GetComponent<Sacrifice>();
        if (sacrifice)
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
