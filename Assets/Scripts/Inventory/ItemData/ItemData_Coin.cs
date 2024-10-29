using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 동전 아이템용 ItemData
/// </summary>
[CreateAssetMenu(fileName = "New Item Data - Coin", menuName = "Scriptable Object/Item Data - Coin", order = 1)]
public class ItemData_Coin : ItemData, IConsumable
{
    public void Consume(GameObject target)
    {
        IMoneyContainer moneyContainer = target.GetComponent<IMoneyContainer>();
        if (moneyContainer != null)     // target이 돈을 담을 수 있으면
        {
            moneyContainer.Money += (int)price; // 돈을 증가시킨다.
        }
    }
}
