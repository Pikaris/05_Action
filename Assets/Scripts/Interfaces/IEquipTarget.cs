using UnityEngine;

public interface IEquipTarget
{
    /// <summary>
    /// 특정 부위에 어느 슬롯에 있는 아이템이 장비되었는지 또는 장비되지 않았는지 확인하기 위한 인덱서
    /// </summary>
    /// <param name="part">확인할 부위</param>
    /// <returns>null이면 장비되어 있지 않음, null이 아니면 해당 슬롯에 있는 장비가 장비되어 있다</returns>
    InvenSlot this[EquipType part] { get; }

    /// <summary>
    /// 아이템을 장비하는 함수
    /// </summary>
    /// <param name="part">장비할 부위</param>
    /// <param name="slot">장비할 아이템이 들어있는 슬롯</param>
    void EquipItem(EquipType part, InvenSlot slot);

    /// <summary>
    /// 아이템을 장비 해제하는 함수
    /// </summary>
    /// <param name="part">장비 해제할 부위</param>
    void UnEquipItem(EquipType part);

    /// <summary>
    /// 장비될 아이템이 추가될 부모 트랜스폼을 찾아주는 함수
    /// </summary>
    /// <param name="part">장비될 부위</param>
    /// <returns>장비될 부위의 부모 트랜스폼</returns>
    Transform GetEquipParentTransform(EquipType part);
}