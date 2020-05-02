using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] float speed = 1;

    private void Update()
    {
        var pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;
    }
}
