using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equip : ItemData, IEquipable
{
    [Header("장비 아이템 데이터")]
    /// <summary>
    /// 아이템을 장비 했을 때 생성해야 할 프리팹
    /// </summary>
    public GameObject equipPrefab;

    /// <summary>
    /// 아이템이 장비될 위치를 알려주는 프로퍼티
    /// </summary>
    public virtual EquipType EquipType => EquipType.Weapon;

    /// <summary>
    /// 아이템을 장비하는 함수
    /// </summary>
    /// <param name="target">장비 받을 대상</param>
    /// <param name="slot">아이템이 들어있는 슬롯</param>
    public void Equip(GameObject target, InvenSlot slot)
    {
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if (equipTarget != null)
        {
            equipTarget.EquipItem(EquipType, slot); // 장비 가능한 대상이면 장비
        }
    }


    /// <summary>
    /// 아이템을 장비 해제하는 함수
    /// </summary>
    /// <param name="target">장비 해제할 대상</param>
    /// <param name="slot">아이템이 들어있는 슬롯</param>
    public void UnEquip(GameObject target, InvenSlot slot)
    {
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if (equipTarget != null)
        {
            equipTarget.UnEquipItem(EquipType); // 장비 가능한 대상이면 장비 해제
        }
    }


    /// <summary>
    /// 아이템을 장비 또는 해제하는 함수
    /// </summary>
    /// <param name="target">장비받거나 해제할 대상</param>
    /// <param name="slot">아이템이 들어있는 슬롯</param>
    public void ToggleEquip(GameObject target, InvenSlot slot)
    {
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if (equipTarget != null)    // 장비 가능한 대상인지 확인
        {
            InvenSlot oldSlot = equipTarget[EquipType]; // 현재 장비 여부 확인
            if (oldSlot != null)
            {
                // 무언가가 장비되어 있다
                UnEquip(target, oldSlot);   // 장비하고 있던 것은 장비 해제
                if(oldSlot != slot)         // 현재 장비하고 있던 슬롯과 새 슬롯이 다르면
                {
                    Equip(target, slot);    // 새 슬롯에 있는 아이템 장비
                }
            }
            else
            {
                // 아무것도 장비되어 있지 않다
                Equip(target, slot);        // 새로 장비하기
            }
        }
    }
}
