using UnityEngine;

public interface IEquipable
{
    /// <summary>
    /// 아이템이 장착될 부위
    /// </summary>
    EquipType EquipType { get; }

    /// <summary>
    /// 아이템을 장비하는 함수
    /// </summary>
    /// <param name="target">장비 받을 대상</param>
    /// <param name="slot">아이템이 들어있는 슬롯</param>
    void Equip(GameObject target, InvenSlot slot);


    /// <summary>
    /// 아이템을 장비 해제하는 함수
    /// </summary>
    /// <param name="target">장비 해제할 대상</param>
    /// <param name="slot">아이템이 들어있는 슬롯</param>
    void UnEquip(GameObject target, InvenSlot slot);


    /// <summary>
    /// 아이템을 장비 또는 해제하는 함수
    /// </summary>
    /// <param name="target">장비받거나 해제할 대상</param>
    /// <param name="slot">아이템이 들어있는 슬롯</param>
    void ToggleEquip(GameObject target, InvenSlot slot);
}