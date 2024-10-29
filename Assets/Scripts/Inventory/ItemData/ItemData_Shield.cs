using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 방패 아이템용 ItemData
/// </summary>
[CreateAssetMenu(fileName = "New Item Data - Shield", menuName = "Scriptable Object/Item Data - Shield", order = 7)]
public class ItemData_Shield : ItemData_Equip
{
    [Header("방패 데이터")]
    /// <summary>
    /// 방패 방어력
    /// </summary>
    public float defencePower = 15.0f;

    /// <summary>
    /// 장비 종류는 방패
    /// </summary>
    public override EquipType EquipType => EquipType.Shield;
}
