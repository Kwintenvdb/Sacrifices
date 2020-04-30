using UnityEngine;
using UnityEngine.EventSystems;

public class Sacrifice : MonoBehaviour, IDragHandler, IPointerClickHandler
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

    // handle going into "dragging mode"
    public void OnDrag(PointerEventData eventData)
    {
        print("drag");
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        print("click");
    }

    private void Update()
    {
        // some queue movement code in here maybe
    }

}
