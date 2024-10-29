using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : BarBase
{
    private void Start()
    {
        PlayerStatus status = GameManager.Instance.Status;
        if(status != null)
        {
            maxValue = status.MaxHP;
            max.text = $" / {maxValue}";
            current.text = status.HP.ToString("f0");
            slider.value = status.HP / status.MaxHP;
            status.onHealthChange += OnValueChange;
        }
    }
}
