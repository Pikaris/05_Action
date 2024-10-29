using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InvenTempSlotUI : SlotUI_Base
{
    Player owner;

    public override void InitializeSlot(InvenSlot slot)
    {
        base.InitializeSlot(slot);

        owner = GameManager.Instance.InventoryUI.Owner;
    }

    private void Update()
    {
        // 항상 마우스 위치에 임시 슬롯을 위치하게 만들기(아이템이 임시 슬롯에 들어왔을 때만 보인다)
        transform.position = Mouse.current.position.ReadValue();
    }

    /// <summary>
    /// screen좌표가 가리키는 월드 포지션에 임시 슬롯에 들어있는 아이템을 드랍하는 함수
    /// </summary>
    /// <param name="screen"></param>
    public void ItemDrop(Vector2 screen)
    {
        // 1. 아이템이 있을 때만 처리
        // 2. screen좌표가 가리키는 바닥(Ground) 주변에 아이템을 드랍
        // 3. 플레이어 위치에서 itemPickupRange반경안에 아이템이 드랍되어야 한다.
        // 4. 아이템을 1개 드랍할 때는 노이즈 없음



        if (!InvenSlot.IsEmpty)
        {
            Ray ray = Camera.main.ScreenPointToRay(screen);     // 레이 구하기
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000.0f, LayerMask.GetMask("Ground"))) // Ground레이어를 가진 물체와 충돌 확인
            {
                Vector3 dropPosition = hitInfo.point;       // 충돌한 위치를 드랍위치로 설정
                dropPosition.y = 0;

                Vector3 dropDir = dropPosition - owner.transform.position;
                if(dropDir.sqrMagnitude > owner.ItemPickupRange * owner.ItemPickupRange)    // 드랍 위치가 너무 멀면
                {
                    // 오너의 위치에서 dropDir방향으로 owner.ItemPickupRange만큼 이동한 위치
                    dropPosition = dropDir.normalized * owner.ItemPickupRange + owner.transform.position;   // 일정 반경안으로 조정
                }

                

                Factory.Instance.MakeItems(     // 아이템 생성
                    InvenSlot.ItemData.code, 
                    InvenSlot.ItemCount, 
                    dropPosition, 
                    InvenSlot.ItemCount > 1);             // 아이템이 1개면 노이즈 없음, 1개 초과면 노이즈 있음
                InvenSlot.ClearSlotItem();      // 슬롯 비우기
            }
        }
    }
}
