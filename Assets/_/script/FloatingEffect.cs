using UnityEngine;
using UnityEngine.UIElements;

public class FloatingEffect : MonoBehaviour
{
    public float speed = 2f;
    public float height = 0.11f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

}
