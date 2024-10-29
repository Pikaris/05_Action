using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 음식 아이템용 ItemData
/// </summary>
[CreateAssetMenu(fileName = "New Item Data - Food", menuName = "Scriptable Object/Item Data - Food", order = 2)]
public class ItemData_Food : ItemData, IConsumable
{
    [Header("음식 아이템 데이터")]
    public float tickRegen = 1.0f;
    public float tickInterval = 1.0f;
    public uint totalTickCount = 3;

    public void Consume(GameObject target)
    {
        IHealth health = target.GetComponent<IHealth>();
        if (health != null)
        {
            health.HealthRegenerateByTick(tickRegen, tickInterval, totalTickCount);     // 음식은 틱당 회복
        }
    }
}
