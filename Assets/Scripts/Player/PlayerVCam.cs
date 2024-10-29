using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerVCam : MonoBehaviour
{
    private void Start()
    {
        CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = GameManager.Instance.Player.transform;
    }
}
