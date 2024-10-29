using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI_Base : MonoBehaviour
{
    /// <summary>
    /// 이 UI가 가지고 있는 슬롯
    /// </summary>
    InvenSlot invenSlot;

    /// <summary>
    /// 슬롯을 확인하기 위한 프로퍼티
    /// </summary>
    public InvenSlot InvenSlot => invenSlot;

    /// <summary>
    /// 슬롯의 인덱스
    /// </summary>
    public uint Index => invenSlot.Index;

    /// <summary>
    /// 아이템의 아이콘을 표시할 UI
    /// </summary>
    Image itemIcon;

    /// <summary>
    /// 아이템의 개수를 표시할 UI
    /// </summary>
    TextMeshProUGUI itemCount;

    protected virtual void Awake()
    {
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();

        child = transform.GetChild(1);
        itemCount = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 슬롯을 초기화 하는 함수(InvenSlot과 SlotUI_Base를 연결하기)
    /// </summary>
    /// <param name="slot"></param>
    public virtual void InitializeSlot(InvenSlot slot)
    {

        invenSlot = slot;   // 슬롯 저장
        invenSlot.ClearDelegates();             // (다시 초기화가 되었을 때를 대비한 코드)
        invenSlot.onSlotItemChange += Refresh;  // 아이템 변경이 있을 때 화면 갱신 
        Refresh();          // 첫 화면 갱신
    }

    /// <summary>
    /// 슬롯 UI에 표시되는 화면 갱신
    /// </summary>
    void Refresh()
    {
        if(InvenSlot.IsEmpty)
        {
            // 슬롯이 비어있다
            itemIcon.color = Color.clear;       // 아이콘 투명하게 만들고
            itemIcon.sprite = null;             // 아이콘 스프라이트 제거
            itemCount.text = string.Empty;      // 텍스트 제거
        }
        else
        {
            // 슬롯에 아이템이 들어있다
            itemIcon.sprite = InvenSlot.ItemData.itemIcon;      // 아이콘 스프라이트 설정
            itemIcon.color = Color.white;                       // 아이콘 보이게 만들기
            itemCount.text = InvenSlot.ItemCount.ToString();    // 아이템 개수 쓰기
        }

        OnRefresh();
    }

    /// <summary>
    /// 화면 갱신 타이밍에 자식 클래스에서 개별적으로 실행할 코드용 함수
    /// </summary>
    protected virtual void OnRefresh()
    {
        // InvenSlotUI에서 장비 여부 표시 갱신용
    }
}
