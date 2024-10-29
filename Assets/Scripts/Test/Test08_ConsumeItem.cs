using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test08_ConsumeItem : Test07_ItemPickUpAndDrop
{
    PlayerStatus status;

    protected override void Awake()
    {
        base.Awake();
    }
    
    protected override void Start()
    {
        base.Start();

        status = player.GetComponent<PlayerStatus>();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        status.HealthHeal(-90);
        status.ManaRestore(-90);
    }

    /// HP와 MP의 현황을 표시하는 UI 만들기
    /// 텍스트로 "현재/최대"를 출력한다 
    /// 슬라이더로 현재/최대 비율을 출력한다.
    /// HP는 빨간색, MP는 파란색으로 표시
    

}
