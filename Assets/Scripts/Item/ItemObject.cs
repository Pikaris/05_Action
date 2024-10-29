using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : RecycleObject
{
    ItemData data = null;
    SpriteRenderer spriteRenderer = null;

    public ItemData ItemData
    {
        get => data;
        set
        {
            if (data == null)       // 활성화 이후에 단 한번만 설정 가능
            {
                data = value;
                spriteRenderer.sprite = data.itemIcon;  // 아이콘 변경
            }
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void OnDisable()
    {
        data = null;
        base.OnDisable();
    }

    /// <summary>
    /// 아이템이 먹혀서 비활성화 되는 함수
    /// </summary>
    public void ItemCollected()
    {
        gameObject.SetActive(false);
    }
}
