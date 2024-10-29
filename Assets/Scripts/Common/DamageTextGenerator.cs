using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextGenerator : MonoBehaviour
{

    // IBattle의 onHit이 발생했을 때 DamageText하나 팩토리에서 생성

    private void Start()
    {
        IBattle battle = transform.parent.GetComponent<IBattle>();
        battle.onHit += DamageTextGenerate;
    }

    void DamageTextGenerate(int damage)
    {
        Factory.Instance.MakeDamageText(damage, transform.position);
    }
}
