using UnityEngine;

public class Sacrifice : MonoBehaviour
{
    // each sacrifice needs some stats:
    // * god favor modifier
    // * people favor modifier
    // * name
    // * flavor text? / description
    // * current position in queue

    [SerializeField] private string sacrificeName;
    [SerializeField] private string description;
    [SerializeField] private float godFavorModifier;
    [SerializeField] private float peopleFavorModifier;
    [SerializeField] private int queuePosition;

    private void Update()
    {
        // some queue movement code in here maybe
    }
}
