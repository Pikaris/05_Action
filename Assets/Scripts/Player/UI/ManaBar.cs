using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBar : BarBase
{
    private void Start()
    {
        PlayerStatus status = GameManager.Instance.Status;
        if (status != null)
        {
            maxValue = status.MaxMP;
            max.text = $" / {maxValue}";
            current.text = status.MP.ToString("f0");
            slider.value = status.MP / status.MaxMP;
            status.onManaChange += OnValueChange;
        }
    }
}
