using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
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
}


