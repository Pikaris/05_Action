using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarBase : MonoBehaviour
{
    /// <summary>
    /// 보여질 색상
    /// </summary>
    public Color color = Color.white;

    protected Slider slider;
    protected TextMeshProUGUI current;
    protected TextMeshProUGUI max;

    /// <summary>
    /// 최대 값
    /// </summary>
    protected float maxValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        Transform child = transform.GetChild(2);
        current = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        max = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(0);
        Image bgImage = child.GetComponent<Image>();
        child = transform.GetChild(1).GetChild(0);
        Image fillImage = child.GetComponent<Image>();

        bgImage.color = new(color.r, color.g, color.b, color.a * 0.5f);
        fillImage.color = color;
    }

    /// <summary>
    /// 값에 변화가 있을 때 실행될 함수
    /// </summary>
    /// <param name="ratio">변화된 비율(0~1 사이)</param>
    protected void OnValueChange(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        slider.value = ratio;
        current.text = $"{(ratio * maxValue):f0}";
    }
}
