using UnityEngine;

public class CoinVisual : MonoBehaviour
{
    public float rotationSpeed = 180f;
    public float floatSpeed = 2f;
    public float floatHeight = 0.25f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(
            0f,
            rotationSpeed * Time.deltaTime,
            0f,
            Space.World
        );

        float yOffset =
            Mathf.Sin(Time.time * floatSpeed) *
            floatHeight;

        transform.position =
            startPos +
            Vector3.up * yOffset;
    }
}