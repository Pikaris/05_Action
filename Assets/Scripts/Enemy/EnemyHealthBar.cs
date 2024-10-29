using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    // 1. 빌보드로 구성한다
    // 2. 적의 HP 변화에 따라 슬라이더가 조절된다.

    Transform fillPivot;


    private void Awake()
    {
        IHealth health = transform.parent.GetComponent<IHealth>();
        health.onHealthChange += Refresh;

        fillPivot = transform.GetChild(1);
    }

    private void Refresh(float ratio)
    {
        fillPivot.localScale = new Vector3(ratio, 1, 1);
    }

    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
