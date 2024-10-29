using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    /// <summary>
    /// 생성 할 때 노이즌 반지름
    /// </summary>
    public float spawnNoise = 0.5f;

    ItemDataManager itemDataManager;

    ItemPool itemPool;
    DamageTextPool damageTextPool;


    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();

        itemDataManager = GetComponent<ItemDataManager>();
    }

    protected override void OnInitialize()
    {
        Transform child = transform.GetChild(0);
        itemPool = child.GetComponent<ItemPool>();
        itemPool?.Initialize();

        child = transform.GetChild(1);
        damageTextPool = child.GetComponent<DamageTextPool>();
        damageTextPool?.Initialize();
    }

    /// <summary>
    /// 아이템을 하나 생성하는 함수(위치, 노이즈 설정 가능)
    /// </summary>
    /// <param name="code">생성할 아이템의 종류</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="useNoise">노이즈 적용 여부(true면 노이즈 적용, false면 적용하지 않음)</param>
    /// <returns>아이템의 게임 오브젝트</returns>
    public GameObject MakeItem(ItemCode code, Vector3? position = null, bool useNoise = false)
    {
        ItemData data = itemDataManager[code];    // 아이템 데이터 매니저에서 ItemData 가져오고
        ItemObject obj = itemPool.GetObject();                  // 풀에서 아이템 오브젝트 하나 꺼낸 후
        obj.ItemData = data;                                    // 아이템 데이터 설정

        obj.transform.position = position.GetValueOrDefault();  // position이 null이면 (0, 0, 0), null이 아니면 설정된 값
        if (useNoise)
        {
            Vector3 noise = Vector3.zero;

            Vector2 rand = Random.insideUnitCircle * spawnNoise;    // 반지름이 spawnNoise인 원 안에 랜덤한 지점 구하기
            noise.x = rand.x;
            noise.z = rand.y;                   // xz평면상의 위치를 구하는거라 변환

            obj.transform.position += noise;    // 노이즈 추가 적용
        }

        return obj.gameObject;  // 게임 오브젝트 리턴
    }

    /// <summary>
    /// 아이템을 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 종류</param>
    /// <param name="count">생성할 아이템의 개수</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="useNoise">노이즈 적용 여부(true면 노이즈 적용, false면 적용하지 않음)</param>
    /// <returns>생성된 아이템의 게임 오브젝트 배열</returns>
    public GameObject[] MakeItems(ItemCode code, uint count, Vector3? position = null, bool useNoise = false)
    {
        GameObject[] items = new GameObject[count];
        for (int i = 0; i < count; i++)     // count만큼 반복해서 MakeItem 호출
        {
            items[i] = MakeItem(code, position, useNoise);
        }
        return items;
    }

    public GameObject MakeDamageText(int damage, Vector3? position = null)
    {
        DamageText damageText = damageTextPool.GetObject(position);
        damageText.SetDamage(damage);
        return damageText.gameObject;
    }
}
