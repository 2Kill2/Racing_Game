using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //move settings
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float turnSpeed = 200f;

    //cam settings
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float cameraHeight = 1.5f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private Camera mainCamera;
    private Transform cameraTransform;

    private Vector3 velocity;
    private bool isGrounded;

    // cam
    private float horizontalAngle = 20f;
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
        if (Physics.Raycast(targetPosition, offset.normalized, out hit, cameraDistance))
        {
            cameraTransform.position = hit.point;
        }
        else
        {
            cameraTransform.position = targetPosition + offset;
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

        if (vertical != 0 && horizontal != 0)
        {
            float turn = horizontal * turnSpeed * Time.deltaTime;
            transform.Rotate(0f, turn, 0f);
        }

        Vector3 moveDirection = transform.forward * vertical;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

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
