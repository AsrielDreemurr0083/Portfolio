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
        if (Input.GetButtonDown("Jump") && isGrounded)
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
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }
}