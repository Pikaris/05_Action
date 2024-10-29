using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyPanelUI : MonoBehaviour
{
    TextMeshProUGUI money;

    private void Awake()
    {
        money = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Refresh(int money)
    {
        this.money.text = money.ToString("N0");
    }
}
