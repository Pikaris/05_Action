using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 음료 아이템용 ItemData
/// </summary>
[CreateAssetMenu(fileName = "New Item Data - Drink", menuName = "Scriptable Object/Item Data - Drink", order = 3)]
public class ItemData_Drink : ItemData, IConsumable
{
    [Header("음료 아이템 데이터")]
    public float totalRegen = 1.0f;
    public float duration = 1.0f;

    public void Consume(GameObject target)
    {
        IMana mana = target.GetComponent<IMana>();
        if (mana != null)
        {
            mana.ManaRegenerate(totalRegen, duration);
        }
    }
}
