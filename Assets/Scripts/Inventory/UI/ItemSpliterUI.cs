using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ItemSpliterUI : MonoBehaviour
{
    // 임시 슬롯이 비어있는 상태에서 일반 슬롯을 쉬프트를 누른채로 클릭하면 열린다
    // 열릴 때 쉬프트 클릭한 슬롯의 아이콘이 설정되어야한다.
    // 기본적으로 나누는 개수가 1로 시작
    // 나눌 수 있는 최소치는 1이고 최대치는 쉬프트 클릭한 슬롯에 들어있는 아이템 개수 -1(인풋필드, 버튼, 슬라이더 3개 다 적용되어야 함)
    // OK버튼을 누르면 나누는 개수만큼 원본 슬롯에서 덜어서 임시슬롯으로 보내고 닫힌다.
    // Cancle버튼을 누르면 그냥 닫힌다.

    /// <summary>
    /// 아이템을 나눌 슬롯
    /// </summary>
    InvenSlot targetSlot;

    /// <summary>
    /// 아이템을 나눌 때 최소 개수
    /// </summary>
    const uint MinItemCount = 1;

    /// <summary>
    /// 아이템을 나눌 때 최대 개수를 확인하기 위한 프로퍼티
    /// </summary>
    uint MaxItemCount => (targetSlot != null) ? (targetSlot.ItemCount - 1) : MinItemCount;

    /// <summary>
    /// 아이템을 나눌 개수
    /// </summary>
    uint count = MinItemCount;

    /// <summary>
    /// 아이템을 나눌 개수를 확인하고 설정하는 프로퍼티
    /// </summary>
    uint Count
    {
        get => count;
        set
        {
            count = Math.Clamp(value, MinItemCount, MaxItemCount);
            inputField.text = count.ToString();     // 인풋 필드
            slider.value = count;                   // 슬라이더
        }
    }

    /// <summary>
    /// OK버튼이 눌려졌음을 알리는 델리게이트(uint : 슬롯의 인덱스, uint : 나눌 개수)
    /// </summary>
    public event Action<uint, uint> onOkClick;

    /// <summary>
    /// Cancel버튼이 눌려졌음을 알리는 델리게이트
    /// </summary>
    public event Action onCancelClick;

    /// <summary>
    /// 휠과 클릭입력을 받기 위한 인풋 액션
    /// </summary>
    PlayerInputActions inputActions;

    // 컴퍼넌트들
    Image icon;
    TMP_InputField inputField;
    Slider slider;

    private void Awake()
    { 
        inputActions = new PlayerInputActions();

        Transform child = transform.GetChild(0);
        icon = child.GetComponent<Image>();

        child = transform.GetChild(1);
        inputField = child.GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener((text) =>
        {
            if(uint.TryParse(text, out uint inputValue))
            {
                Count = inputValue;     // 변환된 값으로 설정
            }
            else
            {
                Count = MinItemCount;   // 음수를 입력했으니 최소값으로 설정
            }
        });

        child = transform.GetChild(2);
        Button plus = child.GetComponent<Button>();
        plus.onClick.AddListener(() => Count++);    // Count 1 증가

        child = transform.GetChild(3);
        Button minus = child.GetComponent<Button>();
        minus.onClick.AddListener(() => Count--);   // Count 1 감소

        child = transform.GetChild(4);
        slider = child.GetComponent<Slider>();
        slider.onValueChanged.AddListener((value) =>
        {
            Count = (uint)value;
        });
        slider.minValue = MinItemCount;

        child = transform.GetChild(5);
        Button ok = child.GetComponent<Button>();
        ok.onClick.AddListener(() =>
        {
            onOkClick?.Invoke(targetSlot.Index, Count);
            Close();
        });

        child = transform.GetChild(6);
        Button cancel = child.GetComponent<Button>();
        cancel.onClick.AddListener(() =>
        {
            onCancelClick?.Invoke();
            Close();
        });

        Close();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += OnClick;
        inputActions.UI.Wheel.performed += OnWheel;
    }

    private void OnDisable()
    {
        inputActions.UI.Wheel.performed -= OnWheel;
        inputActions.UI.Click.performed -= OnClick;
        inputActions.UI.Disable();
    }

    private void OnClick(InputAction.CallbackContext _)
    {
        // UI 밖을 클릭하면 닫힌다
        if(!MousePointInRect())
        {
            Close();
        }
    }
    private void OnWheel(InputAction.CallbackContext context)
    {
        // 휠 움직임에 따라 Count가 증감한다
        if (MousePointInRect())
        {
            if(context.ReadValue<float>() > 0)
            {
                Count++;
            }
            else
            {
                Count--;
            }
        }
    }

    /// <summary>
    /// 마우스 커서 위치가 UI안인지 밖인지 확인하는 함수
    /// </summary>
    /// <returns>UI안이면 true, 밖이면 false</returns>
    bool MousePointInRect()
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector2 diff = screen - (Vector2)transform.position;    // 피봇 기준으로 떨어져 있는 정도

        RectTransform rect = (RectTransform)transform;
        return rect.rect.Contains(diff);                        // RectTransform안쪽이면 true, 아니면 false
    }


    /// <summary>
    /// 아이템 분리창을 여는 함수
    /// </summary>
    /// <param name="target">아이템을 나눌 슬롯</param>
    /// <returns>true면 열렸다, false면 열리지 않았다</returns>
    public bool Open(InvenSlot target)
    {
        bool result = false;
        if(!target.IsEmpty && target.ItemCount > MinItemCount)      // 타켓 슬롯에 아이템이 들어있고 개수가 1개를 초과했을 때만 연다
        {
            targetSlot = target;
            icon.sprite = targetSlot.ItemData.itemIcon;
            slider.maxValue = MaxItemCount;
            Count = targetSlot.ItemCount / 2;

            result = true;
            gameObject.SetActive(true);
        }

        return result;
    }

    /// <summary>
    /// 아이템 분리창을 닫는 함수
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
