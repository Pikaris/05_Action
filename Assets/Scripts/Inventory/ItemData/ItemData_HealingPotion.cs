using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 힐링포션 아이템용 ItemData(최대 HP에 비례한 즉시 회복 + 틱당 회복)
/// </summary>
[CreateAssetMenu(fileName = "New Item Data - HealingPotion", menuName = "Scriptable Object/Item Data - HealingPotion", order = 4)]
public class ItemData_HealingPotion : ItemData, IUsable
{
    [Header("힐링 포션 데이터")]
    /// <summary>
    /// 최대 HP에 비례해서 즉시 회복시켜 주는 양
    /// </summary>
    public float healRatio = 0.3f;

    /// <summary>
    /// 틱당 회복량
    /// </summary>
    public float tickRegen = 3.0f;

    /// <summary>
    /// 틱당 시간 간격
    /// </summary>
    public float tickInterval = 0.3f;

    /// <summary>
    /// 전체 틱 수
    /// </summary>
    public uint totalTickCount = 5;

    public bool Use(GameObject target)
    {
        bool result = false;

        IHealth health = target.GetComponent<IHealth>();
        if (health != null)
        {
            if (health.HP < health.MaxHP)
            {
                health.HealthHeal(health.MaxHP * healRatio);    // 최대 HP의 30%만큼 즉시 증가
                health.HealthRegenerateByTick(tickRegen, tickInterval, totalTickCount); // 틱당 추가로 회복
                result = true;
            }
            else
            {
                Debug.Log($"{target.name}의 HP가 가득 차있습니다. 사용불가");
            }
        }

        return result;
    }
}
