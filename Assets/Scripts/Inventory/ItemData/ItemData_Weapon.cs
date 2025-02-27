using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기 아이템용 ItemData
/// </summary>
[CreateAssetMenu(fileName = "New Item Data - Weapon", menuName = "Scriptable Object/Item Data - Weapon", order = 6)]
public class ItemData_Weapon : ItemData_Equip
{
    [Header("무기 데이터")]
    /// <summary>
    /// 무기 공격력
    /// </summary>
    public float attackPower = 30.0f;

    /// <summary>
    /// 장비 종류는 무기
    /// </summary>
    public override EquipType EquipType => EquipType.Weapon;
}
