using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //move settings
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 8f;
    [SerializeField] private float topSpeed = 5f;
    [SerializeField] private float sprintTopSpeed = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float turnSpeed = 200f;

    //cam settings
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float cameraHeight = 1.5f;
    [SerializeField] private float cameraCollisionOffset = 0.2f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private Camera mainCamera;
    private Transform cameraTransform;

    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed = 0f;

    // cam
    private float horizontalAngle = 0f;
    private float verticalAngle = 20f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
    }

    void LateUpdate()
    {
        moveCamera();
    }

    void moveCamera()
    {
        if (cameraTransform == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        horizontalAngle += mouseX;
        verticalAngle -= mouseY;
        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);

        Vector3 targetPosition = cameraTarget != null ? cameraTarget.position : transform.position + Vector3.up * cameraHeight;

        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);
        Vector3 offset = rotation * new Vector3(0f, 0f, -cameraDistance);

        RaycastHit hit;
        Vector3 desiredPosition = targetPosition + offset;
        Vector3 direction = desiredPosition - targetPosition;

        if (Physics.SphereCast(targetPosition, cameraCollisionOffset, direction.normalized, out hit, cameraDistance))
        {
            float distance = Mathf.Max(hit.distance - cameraCollisionOffset, 0.5f);
            cameraTransform.position = targetPosition + direction.normalized * distance;
        }
        else
        {
            cameraTransform.position = desiredPosition;
        }

        cameraTransform.LookAt(targetPosition);
    }

    void Move()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float targetTopSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintTopSpeed : topSpeed;

        if (vertical != 0)
        {
            float targetSpeed;
            if (vertical > 0)
            {
                targetSpeed = targetTopSpeed;
            }
            else
            {
                targetSpeed = -targetTopSpeed;
            }

            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        if (Mathf.Abs(currentSpeed) > 0.1f && horizontal != 0)
        {
            float turn = horizontal * turnSpeed * Time.deltaTime;
            transform.Rotate(0f, turn, 0f);
        }

        Vector3 moveDirection = transform.forward * currentSpeed;

        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime; //gravity
        controller.Move(velocity * Time.deltaTime);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
