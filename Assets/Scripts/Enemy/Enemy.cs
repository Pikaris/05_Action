using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Serializable]
    public struct ItemDropInfo
    {
        public ItemCode code;       // 어떤 종류의 아이템인지
        [Range(0f, 1f)]
        public float dropRatio;     // 드랍될 확률은 얼마인지
        public uint dropCount;      // 최대 몇개를 드랍할 것인지
    }

    public ItemDropInfo[] dropItems;

    public void DropItems()
    {
        foreach (var item in dropItems)
        {
            if(item.dropRatio > Random.value)
            {
                uint count = (uint)Random.Range(0, (int)item.dropCount) + 1;
                Factory.Instance.MakeItems(item.code, count, transform.position, true);
            }
        }
    }
}
