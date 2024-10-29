#define PrintTestLog

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvenSlotUI : SlotUI_Base, IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    /// <summary>
    /// 드래그의 시작을 알리는 델리게이트(uint : 드래그를 시작한 슬롯의 인덱스)
    /// </summary>
    public event Action<uint> onDragBegin;

    /// <summary>
    /// 드래그의 종료를 알리는 델리게이트(uint : 드래그가 끝난 슬롯의 인덱스(null이면 슬롯이 아닌 곳에서 종료))
    /// </summary>
    public event Action<uint?> onDragEnd;

    /// <summary>
    /// 마우스 클릭을 알리는 델리게이트(uint : 클릭된 슬롯의 인덱스)
    /// </summary>
    public event Action<uint> onClick;

    /// <summary>
    /// 마우스 커서가 슬롯 위로 올라왔음을 알리는 델리게이트(uint : 올라간 슬롯의 인덱스)
    /// </summary>
    public event Action<uint> onPointerEnter;

    /// <summary>
    /// 마우스 커서가 슬롯 밖으로 나갔음을 알리는 델리게이트
    /// </summary>
    public event Action onPointerExit;

    /// <summary>
    /// 마우스 커서가 슬롯 위에서 움직임을 알리는 델리게이트(Vector2 : 마우스 포인터의 스크린좌표)
    /// </summary>
    public event Action<Vector2> onPointerMove;

    /// <summary>
    /// 
    /// </summary>
    TextMeshProUGUI equipText;

    protected override void Awake()
    {
        base.Awake();
        Transform child = transform.GetChild(2);
        equipText = child.GetComponent<TextMeshProUGUI>();
    }

    public override void InitializeSlot(InvenSlot slot)
    {
        ClearDelegates();
        base.InitializeSlot(slot);
    }

    protected override void OnRefresh()
    {
        base.OnRefresh();
        if (InvenSlot.IsEquipped)
        {
            equipText.color = Color.red;    // 장비했을 때는 빨간색
        }
        else
        {
            equipText.color = Color.clear;  // 장비하지 않았을 때는 투명색
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
#if PrintTestLog
        Debug.Log($"드래그 시작 : [{Index}]번 슬롯");
#endif
        onDragBegin?.Invoke(Index);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // OnBeginDrag와 OnBeginDrag를 발동시키기 위해 필요
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;   // 드래그 끝난 위치에 있는 게임 오브젝트

        if (obj != null)
        {
            // 마우스 위치에 어떤 것이 있다
            InvenSlotUI endSlot = obj.GetComponent<InvenSlotUI>();
            uint? endIndex = null;
            if (endSlot != null)
            {
                // 슬롯이다
                endIndex = endSlot.Index;
#if PrintTestLog
                Debug.Log($"드래그 종료 : [{endIndex}]번 슬롯");
#endif
            }
#if PrintTestLog
            else
            {
                // 슬롯이 아니다
                Debug.Log($"드래그 종료 : [{obj.name}]은 슬롯이 아닙니다");
            }
#endif
            onDragEnd?.Invoke(endIndex);
        }
        else
        {
            // 마우스 위치에 어떤 게임 오브젝트도 없다
#if PrintTestLog
            Debug.Log("어떤 UI도 없다");
#endif
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(Index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(Index);
    }

    public void ClearDelegates()
    {
        onClick = null;
        onDragBegin = null;
        onDragEnd = null;
        onPointerEnter = null;
        onPointerExit = null;
        onPointerMove = null;
    }
}
