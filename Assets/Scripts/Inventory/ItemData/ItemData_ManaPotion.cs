using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마나포션 아이템용 ItemData(지속적으로 회복)
/// </summary>
[CreateAssetMenu(fileName = "New Item Data - ManaPotion", menuName = "Scriptable Object/Item Data - ManaPotion", order = 5)]
public class ItemData_ManaPotion : ItemData, IUsable
{
    [Header("마나포션 아이템 데이터")]
    public float totalRegen = 50.0f;
    public float duration = 1.0f;

    public bool Use(GameObject target)
    {
        bool result = false;
        IMana mana = target.GetComponent<IMana>();
        if (mana != null)
        {
            if(mana.MP < mana.MaxMP)
            {
                mana.ManaRegenerate(totalRegen, duration);
                result = true;
            }
            else
            {
                Debug.Log($"{target.name}의 MP가 가득 차있습니다. 사용불가");
            }
        }


        return result;
    }
}
