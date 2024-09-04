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
    private bool canJump = true;  // 새로운 변수 추가
    private Camera playerCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        // Rigidbody 설정
        if (rb != null)
        {
            rb.freezeRotation = true; // 물리 회전을 비활성화
        }
    }

    void Update()
    {
        // 마우스 입력 처리
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 좌우 회전
        transform.Rotate(Vector3.up * mouseX);

        // 상하 회전 (카메라만)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // 점프 입력 처리
        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // 이동 입력 처리
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;

        // 캐릭터 이동
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        canJump = false;  // 점프 후 즉시 canJump를 false로 설정
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
        canJump = true;  // 지면에 닿았을 때 canJump를 true로 설정
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        canJump = false;  // 지면에서 떨어졌을 때 canJump를 false로 설정
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

        // 디버그 정보 출력
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
        // 레이캐스트 시작점을 캐릭터의 발 근처로 조정
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;

        // 레이캐스트 수행
        RaycastHit hit;
        bool raycastHit = Physics.Raycast(rayStart, Vector3.down, out hit, groundCheckDistance + 0.1f, groundMask);

        // 디버그 레이 그리기
        Debug.DrawRay(rayStart, Vector3.down * (groundCheckDistance + 0.1f), raycastHit ? Color.green : Color.red);

        // 결과 로깅
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