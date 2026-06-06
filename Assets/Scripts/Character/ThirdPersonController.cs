using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float gravity = -9.81f;

    [Header("References")]
    public Transform cameraTransform;

    [Header("Player State")]
    public bool canMove = true;

    private CharacterController controller;
    private Animator animator;
    private float verticalVelocity;
    private bool wasMoving;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (animator != null)
            animator.applyRootMotion = false;

        AudioManager.instance?.PlayIdle();
    }

    private void Update()
    {
        if (!canMove)
        {
            if (animator != null)
                animator.SetFloat("Speed", 0f);

            AudioManager.instance?.StopAllAudio();
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical);

        float inputMagnitude =
            Mathf.Clamp01(inputDirection.magnitude);

        if (animator != null)
        {
            animator.SetFloat(
                "Speed",
                inputMagnitude,
                0.1f,
                Time.deltaTime
            );
        }

        bool isMoving = inputMagnitude > 0.1f;

        if (isMoving && !wasMoving)
        {
            AudioManager.instance?.PlayWalk();
        }
        else if (!isMoving && wasMoving)
        {
            AudioManager.instance?.PlayIdle();
        }

        wasMoving = isMoving;

        if (isMoving)
        {
            float targetAngle =
                Mathf.Atan2(
                    inputDirection.x,
                    inputDirection.z
                ) *
                Mathf.Rad2Deg +
                cameraTransform.eulerAngles.y;

            Quaternion targetRotation =
                Quaternion.Euler(
                    0f,
                    targetAngle,
                    0f
                );

            transform.rotation =
                Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );

            Vector3 moveDirection =
                Quaternion.Euler(
                    0f,
                    targetAngle,
                    0f
                ) *
                Vector3.forward;

            controller.Move(
                moveDirection *
                moveSpeed *
                Time.deltaTime
            );
        }

        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        controller.Move(
            new Vector3(
                0f,
                verticalVelocity,
                0f
            ) *
            Time.deltaTime
        );
    }
}