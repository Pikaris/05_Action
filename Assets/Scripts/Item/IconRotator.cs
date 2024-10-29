using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IconRotator : MonoBehaviour
{
    public float rotateSpeed = 360.0f;
    public float moveSpeed = 2.0f;

    public float minHeight = 0.5f;
    public float maxHeight = 1.5f;

    float timeElapsed = 0.0f;

    private void Start()
    {
        transform.Rotate(0, Random.Range(0, 360.0f), timeElapsed);                  // 초기 랜덤 회전
        transform.position = transform.parent.position + Vector3.up * maxHeight;    // 시작 위치 설정
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime * moveSpeed;

        // 위치 설정
        Vector3 pos;
        pos.x = transform.parent.position.x;
        pos.y = minHeight + (Mathf.Cos(timeElapsed) + 1 * 0.5f) * (maxHeight - minHeight);      // 범위 : 0.5 ~ 1.5
        pos.z = transform.parent.position.z;

        transform.position = pos;

        // 회전
        transform.Rotate(0, Time.deltaTime * rotateSpeed, timeElapsed);
    }
}
