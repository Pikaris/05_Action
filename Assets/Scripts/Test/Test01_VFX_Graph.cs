using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Test01_VFX_Graph : TestBase
{
    public VisualEffect effect;

    public float duration = 3.0f;

    readonly int OnStartEventID = Shader.PropertyToID("OnStart");
    readonly int DurationID = Shader.PropertyToID("Duration");

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        effect.SendEvent(OnStartEventID);       // 이벤트 보내기(내가 원하는 타이밍에 이펙트 동작 시킬 수 있음)
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        effect.SetFloat(DurationID, duration);  // 이펙트가 가진 프로퍼티 값 변경
    }
}
