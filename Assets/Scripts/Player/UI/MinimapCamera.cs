using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    /// <summary>
    /// 미니맵 카메라 움직임의 뎀핑 정도
    /// </summary>
    public float damping = 3.0f;

    /// <summary>
    /// 움직일 때 미니맵의 크기
    /// </summary>
    public float movingSize = 10.0f;

    /// <summary>
    /// 줌인/아웃이 일어나는 속도
    /// </summary>
    public float zoomSpeed = 1.0f;

    /// <summary>
    /// 플레이어와 미니맵 카메라가 떨어진 정도
    /// </summary>
    Vector3 offset;

    /// <summary>
    /// 따라다닐 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 미니맵 카메라의 목표 사이즈
    /// </summary>
    float targetSize = defaultSize;

    /// <summary>
    /// 카메라가 플레이어 위치에서 어느정도 높이에 있는지
    /// </summary>
    const float cameraHeight = 30.0f;

    /// <summary>
    /// 미니맵 카메라의 기본 크기
    /// </summary>
    const float defaultSize = 20.0f;

    /// <summary>
    /// 미니맵 카메라
    /// </summary>
    Camera minimapCamera;
 

    private void Start()
    {
        player = GameManager.Instance.Player;
        transform.position = player.transform.position + Vector3.up * cameraHeight; // 초기 위치 설정하기
        offset = transform.position - player.transform.position;                    // 옵센 저장하기

        minimapCamera = GetComponent<Camera>();
        minimapCamera.orthographicSize = defaultSize;   // 카메라 크기 설정
        targetSize = defaultSize;                       // 목표 크기 설정
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, damping * Time.deltaTime);    // 카메라가 플레이어 따라다니게 만들기(댐핑 적용)

        if ((targetPosition - transform.position).sqrMagnitude > 0.01f)     // 움직임 판단
        {
            targetSize = movingSize;        // 움직이는 중이면 movingSize 사용
        }
        else
        {
            targetSize = defaultSize;       // 움직이지 않는 중이면 defaultSize 사용
        }

        // 줌 처리
        minimapCamera.orthographicSize = Mathf.Lerp(minimapCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }
}


// 움직이면 카메라 사이즈가 10 Rkwl w루\
