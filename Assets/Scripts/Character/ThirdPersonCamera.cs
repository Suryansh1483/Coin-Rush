using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    public float distance = 5f;
    public float height = 2f;
    public float sensitivity = 3f;
    public float smoothSpeed = 10f;

    [Header("Pitch Limits")]
    public Vector2 pitchLimits = new Vector2(-30f, 60f);

    private float yaw;
    private float pitch;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("ThirdPersonCamera: Target not assigned.");
            enabled = false;
            return;
        }

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        if (Time.timeScale > 0f)
        {
            float currentSensitivity = PlayerPrefs.GetFloat("MouseSensitivity",sensitivity);

            yaw +=
                Input.GetAxis("Mouse X") *
                currentSensitivity;

            pitch -=
                Input.GetAxis("Mouse Y") *
                currentSensitivity;

            pitch = Mathf.Clamp(
                pitch,
                pitchLimits.x,
                pitchLimits.y
            );
        }

        Quaternion rotation =
            Quaternion.Euler(
                pitch,
                yaw,
                0f
            );

        Vector3 desiredPosition =
            target.position
            - rotation * Vector3.forward * distance
            + Vector3.up * height;

        transform.position =
            Vector3.Lerp(
                transform.position,
                desiredPosition,
                smoothSpeed * Time.deltaTime
            );

        transform.rotation = rotation;
    }
}