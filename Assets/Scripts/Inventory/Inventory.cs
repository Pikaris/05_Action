//#define PrintTestLog

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Inventory
{
    /// <summary>
    /// 인벤토리의 기본 슬롯 수(6칸)
    /// </summary>
    const int Default_Inventory_Size = 6;

    /// <summary>
    /// 인벤토리의 슬롯들(아이템 한 종류가 들어간다)
    /// </summary>
    InvenSlot[] slots;

    /// <summary>
    /// 인벤토리의 슬롯의 개수
    /// </summary>
    int SlotCount => slots.Length;

    /// <summary>
    /// 임시 슬롯(드래그나 아이템 분리작업에서 사용)
    /// </summary>
    InvenTempSlot tempSlot;

    /// <summary>
    /// 아이템 데이터 매니저(아이템 종류별 정보를 전부 가지고 있다)
    /// </summary>
    ItemDataManager itemDataManager;

    /// 인벤토리의 소유자
    /// </summary>
    Player owner;

    /// <summary>
    /// 소유자 확인용 프로퍼티
    /// </summary>
    public Player Owner => owner;

    /// <summary>
    /// 인벤토리 슬롯에 접근하기 위한 인덱서
    /// </summary>
    /// <param name="index">슬롯의 인덱스</param>
    /// <returns>인덱스 번째의 슬롯</returns>
    public InvenSlot this[uint index] => slots[index];

    /// <summary>
    /// 임시 슬롯 확인용 프로퍼티
    /// </summary>
    public InvenTempSlot TempSlot => tempSlot;

    /// <summary>
    /// 인벤토리 생성자
    /// </summary>
    /// <param name="owner">인벤토리 소유자</param>
    /// <param name="size">인벤토리 크기(기본값은 6)</param>
    public Inventory(Player owner, uint size = Default_Inventory_Size)
    {
        slots = new InvenSlot[size];
        for (uint i = 0; i < slots.Length; i++)
        {
            slots[i] = new InvenSlot(i);
        }
        tempSlot = new InvenTempSlot();
        itemDataManager = GameManager.Instance.ItemData;    // 타이밍 조심 필요

        this.owner = owner;
    }

    /// <summary>
    /// 인벤토리의 특정 슬롯에 아이템을 하나 추가하는 함수
    /// </summary>
    /// <param name="code">추가할 아이템의 종류</param>
    /// <param name="slotIndex">아이템을 추가할 슬롯의 인덱스</param>
    /// <returns>true면 추가 성공, false면 추가 실패</returns>
    public bool AddItem(ItemCode code, uint slotIndex)
    {
        bool result = false;

        if (IsValidIndex(slotIndex, out InvenSlot slot))        // 인덱스가 정상 범위인지 확인하고 슬롯 받아오기
        {
            // 인덱스가 정상이다
            ItemData itemData = itemDataManager[code];      // 아이템 데이터 찾아 놓기
            if(slot.IsEmpty)                        // 슬롯이 비어있는지 확인
            {
                slot.AssignSlotItem(itemData);      // 슬롯이 비어있으면 아이템 설정
                result = true;
            }
            else 
            {
                // 슬롯이 비어있지 않다
                if(slot.ItemData == itemData)                   // 같은 종류의 아이템인지 확인
                {
                    // 같은 종류의 아이템이면
                    result = slot.IncreaseSlotItem(out _);      // 아이템 증가 시도(증가되면 true, 안되면 false)
                }
                else
                {
                    // 다른 종류의 아이템이 들어있다
#if PrintTestLog
                    Debug.Log($"아이템 추가 실패 : [{slotIndex}]번 슬롯에는 다른 아이템이 들어있습니다");
#endif
                }
            }
        }
        else
        {
#if PrintTestLog
            Debug.Log($"아이템 추가 실패 : [{slotIndex}]는 잘못된 인덱스입니다");
#endif
        }

        return result;
    }

    /// <summary>
    /// 인벤토리에 아이템을 하나 추가하는 함수
    /// </summary>
    /// <param name="code">추가할 아이템의 종류</param>
    /// <returns>true면 추가 성공, false면 추가 실패</returns>
    public bool AddItem(ItemCode code)
    {
        bool result = false;
        for (uint i = 0; i < SlotCount; i++)
        {
            if(AddItem(code, i))
            {
                result = true;
                break;
            }
        }
        return result;
    }
    // 아이템 삭제(슬롯 일부 삭제, 슬롯 삭제, 인벤토리 전체 삭제)

    /// <summary>
    /// 인벤토리의 특정 슬롯에서 아이템을 일정 개수만큼 제거하는 함수
    /// </summary>
    /// <param name="slotIndex">아이템 개수를 감소시킬 슬롯</param>
    /// <param name="decreaseCount">감소시킬 개수</param>
    public void RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        if (IsValidIndex(slotIndex, out InvenSlot slot))        // 인덱스가 정상 범위인지 확인하고 슬롯 받아오기
        {
            slot.DecreaseSlotItem(decreaseCount);   // 아이템 감소 시도
        }
        else
        {
#if PrintTestLog
            Debug.Log($"아이템 감소 실패 : [{slotIndex}]는 잘못된 인덱스입니다");
#endif
        }
    }

    /// <summary>
    /// 인벤토리의 특정 슬롯을 비우는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 비울 슬롯의 인덱스</param>
    public void ClearSlot(uint slotIndex)
    {
        if (IsValidIndex(slotIndex, out InvenSlot slot))        // 인덱스가 정상 범위인지 확인하고 슬롯 받아오기
        {
            slot.ClearSlotItem();       // 해당 슬롯 비우기
        }
        else
        {
#if PrintTestLog
            Debug.Log($"슬롯 비우기 실패 : [{slotIndex}]는 잘못된 인덱스입니다");
#endif
        }
    }

    /// <summary>
    /// 인벤토리의 전체 슬롯을 비우는 함수
    /// </summary>
    public void ClearInventory()
    {
        foreach(var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }

    /// <summary>
    /// 인벤토리의 from슬롯에 있는 아이템을 to 위치로 옮기는 함수
    /// </summary>
    /// <param name="from">위치 변경 시작 인덱스</param>
    /// <param name="to">위치 변경 도착 인덱스</param>
    public void MoveItem(uint from, uint to)
    {
        if (from != to                                       // from과 to가 다른 슬롯이고 각각 적절한 슬롯일 때
            && IsValidIndex(from, out InvenSlot fromSlot)
            && IsValidIndex(to, out InvenSlot toSlot))
        {
            if(!fromSlot.IsEmpty)   // from에는 반드시 아이템이 들어 있어야 한다
            {
                if (toSlot is InvenTempSlot)
                {
                    TempSlot.FromIndex = from;     // toSlot이 임시 슬롯이면 FromIndex에 돌아갈 위치 설정
                }

                if(fromSlot.ItemData == toSlot.ItemData)
                {
                    // from과 to에 같은 아이템이 들어있는 경우
#if PrintTestLog
                    Debug.Log($"아이템 이동 : [{from}]슬롯에서 [{to}]슬롯으로 아이템 합치기");
#endif

                    toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // to에 from이 가지고 있는 개수 만큼 증가 시도
                    fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);          // to로 넘어간 양 만큼만 감소
                }
                else
                {
                    // from과 to에 다른 아이템이 들어있는 경우
                    if(fromSlot is InvenTempSlot)
                    {

                        // from이 임시 슬롯이다(=to는 일반 슬롯)
                        // => temp슬롯에서 to슬롯으로 아이템을 옮기는 경우 => 드래그가 끝나는 상황
                        // (일반적인 아이템 이동 마무리 상황)
                        // (임시 슬롯에 있던 것이 to에 들어가고 to에 있던 것이 드래그 시작한 슬롯으로 돌아가야 하는 상황)

                        if (TempSlot.FromIndex != null)
                        {
                            InvenSlot dragStartSlot = slots[TempSlot.FromIndex.Value];     // 드래그 시작한 슬롯 찾기

                            if (dragStartSlot.IsEmpty)
                            {
                                // dragStartSlot이 비어 있다(드래그로 아이템 옮기기)
                                dragStartSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount, toSlot.IsEquipped); // dragStartSlot에는 to 슬롯에 있는 아이템 옮기기
                                toSlot.AssignSlotItem(TempSlot.ItemData, TempSlot.ItemCount, TempSlot.IsEquipped);  // to 슬롯에는 임시 슬롯에 있는 아이템 옮기기
                                TempSlot.ClearSlotItem();
                            }
                            else
                            {
                                // dragStartSlot에 아이템이 있다(dragStartSlot 슬롯에서 아이템을 덜어낸 상황)
                                if (dragStartSlot.ItemData == toSlot.ItemData)
                                {
                                    dragStartSlot.IncreaseSlotItem(out uint overCount, toSlot.ItemCount);
                                    toSlot.DecreaseSlotItem(toSlot.ItemCount - overCount);
                                }
                                SwapSlot(TempSlot, toSlot);
                            }
                        }
                        else
                        {
                            SwapSlot(fromSlot, toSlot); // fromIndex가 비어있는 상황이면 그냥 스왑
                        }
                    }
                    else
                    {
                        // fromSlot이 임시 슬롯이 아닌 경우(toSlot이 임시 슬롯)
#if PrintTestLog
                        Debug.Log($"아이템 이동 : [{from}]슬롯과 [{to}]슬롯을 서로 스왑");
#endif
                        SwapSlot(fromSlot, toSlot);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 인벤토리의 특정 슬롯에서 아이템을 일정량 분리하여 임시 슬롯으로 보내는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 분리할 슬롯</param>
    /// <param name="count">분리할 개수</param>
    public void SplitItem(uint slotIndex, uint count)
    {
        if(IsValidIndex(slotIndex, out InvenSlot slot))
        {
            count = Math.Min(count, slot.ItemCount);    // count가 슬롯에 들어있는 개수보다 크다면 슬롯에 들어있는 개수까지만 처리

            TempSlot.AssignSlotItem(slot.ItemData, count);  // 임시 슬롯에 정해진 대로 넣고
            TempSlot.FromIndex = slotIndex;                 // from 저장하고
            slot.DecreaseSlotItem(count);                   // 대상 슬롯에서 감소

#if PrintTestLog
            Debug.Log($"아이템 분리 : [{slotIndex}]슬롯에서 [{count}]개 분리");
#endif
        }
    }

    /// <summary>
    /// 슬롯간에 스왑을 하는 함수
    /// </summary>
    /// <param name="slotA">대상1</param>
    /// <param name="slotB">대상2</param>
    void SwapSlot(InvenSlot slotA, InvenSlot slotB)
    {
        ItemData tempData = slotA.ItemData;
        uint tempCount = slotA.ItemCount;
        bool tempEquip = slotA.IsEquipped;
        slotA.AssignSlotItem(slotB.ItemData, slotB.ItemCount, slotB.IsEquipped);
        slotB.AssignSlotItem(tempData, tempCount, tempEquip);

        // 임시 슬롯은 이제 FromIndex를 비운다
        InvenTempSlot tempA = slotA as InvenTempSlot;
        if(tempA != null)
        {
            tempA.FromIndex = null;
        }
        InvenTempSlot tempB = slotB as InvenTempSlot;
        if(tempB != null)
        {
            tempB.FromIndex = null;
        }
#if PrintTestLog
        Debug.Log($"슬롯 스왑 : [{slotA.Index}]슬롯과 [{slotB.Index}]슬롯을 서로 스왑");
#endif
    }


    /// <summary>
    /// 슬롯 인덱스가 적절한 인덱스 인지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스 번호</param>
    /// <param name="targetSlot">index가 가리키는 슬롯</param>
    /// <returns>존재하는 인덱스면 true, 아니면 false</returns>
    bool IsValidIndex(uint index, out InvenSlot targetSlot)
    {
        // index가 (0~SlotCount-1)이거나 임시 슬롯의 인덱스이면 true
        
        targetSlot = null;

        if (index < SlotCount)
        {
            targetSlot = slots[index];
        }
        else if(index == InvenTempSlot.TempSlotIndex)
        {
            targetSlot = TempSlot;
        }

        return targetSlot != null;
    }
    // 아이템 덜어내기 임시슬롯에 저장(UI때 작업)

    // 인벤토리 내 아이템 정렬

    /// <summary>
    /// 인벤토리를 정렬하는 함수
    /// </summary>
    /// <param name="sortCriteria">정렬 기준</param>
    /// <param name="isAscending">true면 오름차순, false면 내림차순</param>
    public void SlotSorting(ItemSortCriteria sortCriteria, bool isAscending = true)
    {
        List<InvenSlot> sortList = new List<InvenSlot>(slots);      // 정렬용 리스트 만들기(slots 기반)

        switch (sortCriteria)   // 정렬 방법에 따라 스위치 처리
        {
            case ItemSortCriteria.Code:
                sortList.Sort((current, other) =>
                {
                    if (current.ItemData == null)   // 비어있는 슬롯은 뒤로 보내기
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if(isAscending)
                        return current.ItemData.code.CompareTo(other.ItemData.code);
                    else
                        return other.ItemData.code.CompareTo(current.ItemData.code);
                });
                break;
            case ItemSortCriteria.Name:
                sortList.Sort((current, other) =>
                {
                    if (current.ItemData == null)   // 비어있는 슬롯은 뒤로 보내기
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if (isAscending)
                        return current.ItemData.itemName.CompareTo(other.ItemData.itemName);
                    else
                        return other.ItemData.itemName.CompareTo(current.ItemData.itemName);
                });
                break;
            case ItemSortCriteria.Price:
                sortList.Sort((current, other) =>
                {
                    if (current.ItemData == null)   // 비어있는 슬롯은 뒤로 보내기
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if (isAscending)
                        return current.ItemData.price.CompareTo(other.ItemData.price);
                    else
                        return other.ItemData.price.CompareTo(current.ItemData.price);
                });
                break;
            default:
                break;
        }

        // 정렬된 데이터를 기준으로 데이터 따로 저장(직접 sortList를 사용하면 데이터가 섞이게 된다(참조를 저장하고 있기 때문에))
        List<(ItemData, uint, bool)> sortedData = new List<(ItemData, uint, bool)>(SlotCount);
        foreach (var slot in sortList)
        {
            sortedData.Add((slot.ItemData, slot.ItemCount, slot.IsEquipped));
        }
        int index = 0;
        foreach(var data in sortedData)
        {
            slots[index].AssignSlotItem(data.Item1, data.Item2, data.Item3);    // 따로 저장한 내용 순서대로 슬롯에 설정
            index++;
        }
    }

    /// <summary>
    /// 인벤토리에서 같은 종류의 아이템을 최대한 합치는 작업을 하는 함수
    /// </summary>
    public void MergeItems()
    {
        uint count = (uint)(slots.Length - 1);
        for(uint i = 0; i < count; i++)     // i는 0 -> 4
        {
            InvenSlot slot = slots[i];
            for(uint j = count; j > i; j--) // j는 5 -> i+1
            {
                if(slot.ItemData == slots[j].ItemData)
                {
                    MoveItem(j, i);         // 일단 j에 있는 것을 i에 추가하기
                    if (!slots[j].IsEmpty)  // 추가했는데 capacity때문에 j에 아이템이 남았다면
                    {
                        SwapSlot(slots[i + 1], slots[j]);   // i다음 칸과 j를 스왑하기(같은 종류의 아이템을 한군데에 뭉치기 위해)
                        break;
                    }
                }
            }
        }
    }

    // 인벤토리 정리

#if UNITY_EDITOR
    public void Test_InventoryPrint()
    {
        // [ 루비(1/10), 사파이어(3/3), (빈칸), 에메랄드(3/5), (빈칸), (빈칸) ]
        string printText = "[ ";

        for (int i = 0; i < SlotCount - 1; i++)
        {
            if (slots[i].IsEmpty)
            {
                printText += "(빈칸)";
            }
            else
            {
                printText += $"{slots[i].ItemData.itemName}({slots[i].ItemCount}/{slots[i].ItemData.maxStackCount})";
            }
            printText += ", ";
        }
        InvenSlot last = slots[SlotCount - 1];
        if(last.IsEmpty )
        {
            printText += "(빈칸)";
        }
        else
        {
            printText += $"{last.ItemData.itemName}({last.ItemCount}/{last.ItemData.maxStackCount})";
        }
        printText += " ]";
        Debug.Log(printText);
    }
#endif
}
