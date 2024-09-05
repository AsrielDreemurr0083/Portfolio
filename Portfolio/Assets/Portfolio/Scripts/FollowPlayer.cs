using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform ������Ʈ
    public float speed = 5f; // ĸ���� �̵� �ӵ�

    void Update()
    {
        if (player != null)
        {
            // �÷��̾� �������� �̵�
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // �÷��̾ ���� ȸ�� 
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // y�� ȸ���� ���
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
            }
        }
    }
}