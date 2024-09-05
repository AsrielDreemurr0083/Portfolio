using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform 컴포넌트
    public float speed = 5f; // 캡슐의 이동 속도

    void Update()
    {
        if (player != null)
        {
            // 플레이어 방향으로 이동
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // 플레이어를 향해 회전 
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // y축 회전만 고려
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
            }
        }
    }
}