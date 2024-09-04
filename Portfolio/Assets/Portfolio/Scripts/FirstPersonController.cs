using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
/*{
    public float mouseSensitivity = 2f;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private float verticalRotation = 0f;
    private bool isGrounded = true;
    private bool canJump = true;  // ���ο� ���� �߰�
    private Camera playerCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        // Rigidbody ����
        if (rb != null)
        {
            rb.freezeRotation = true; // ���� ȸ���� ��Ȱ��ȭ
        }
    }

    void Update()
    {
        // ���콺 �Է� ó��
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // �¿� ȸ��
        transform.Rotate(Vector3.up * mouseX);

        // ���� ȸ�� (ī�޶�)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // ���� �Է� ó��
        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // �̵� �Է� ó��
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;

        // ĳ���� �̵�
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        canJump = false;  // ���� �� ��� canJump�� false�� ����
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
        canJump = true;  // ���鿡 ����� �� canJump�� true�� ����
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        canJump = false;  // ���鿡�� �������� �� canJump�� false�� ����
    }
}*/


{
    public float mouseSensitivity = 2f;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundMask;
    private Rigidbody rb;
    private float verticalRotation = 0f;
    private bool isGrounded = true;
    private bool canJump = true;
    private Camera playerCamera;
    private float lastJumpTime;
    public float jumpCooldown = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        HandleMouseLook();
        HandleJumpInput();

        // ����� ���� ���
        Debug.Log($"IsGrounded: {isGrounded}, CanJump: {canJump}, LastJumpTime: {lastJumpTime}");
    }

    void FixedUpdate()
    {
        HandleMovement();
        CheckGrounded();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump button pressed");
            if (isGrounded && canJump && Time.time > lastJumpTime + jumpCooldown)
            {
                Jump();
            }
            else
            {
                Debug.Log($"Jump conditions not met: IsGrounded={isGrounded}, CanJump={canJump}, CooldownPassed={Time.time > lastJumpTime + jumpCooldown}");
            }
        }
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Jump()
    {
        Debug.Log("Jumping!");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        canJump = false;
        lastJumpTime = Time.time;
    }

    void CheckGrounded()
    {
        // ����ĳ��Ʈ �������� ĳ������ �� ��ó�� ����
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;

        // ����ĳ��Ʈ ����
        RaycastHit hit;
        bool raycastHit = Physics.Raycast(rayStart, Vector3.down, out hit, groundCheckDistance + 0.1f, groundMask);

        // ����� ���� �׸���
        Debug.DrawRay(rayStart, Vector3.down * (groundCheckDistance + 0.1f), raycastHit ? Color.green : Color.red);

        // ��� �α�
        if (raycastHit)
        {
            Debug.Log($"Ground detected: distance={hit.distance}, layer={LayerMask.LayerToName(hit.collider.gameObject.layer)}");
        }
        else
        {
            Debug.Log("No ground detected");
        }

        isGrounded = raycastHit;
        if (isGrounded)
        {
            canJump = true;
        }
    }
}