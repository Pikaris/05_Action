using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 한 종류의 정보를 저장하는 클래스
/// </summary>
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("아이템 기본 정보")]
    public ItemCode code = ItemCode.Misc;
    public string itemName = "아이템";
    public string itemDescription = "아이템 설명";
    public Sprite itemIcon;
    public uint price = 0;

    [Min(1)]
    public uint maxStackCount = 1;
}
