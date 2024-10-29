using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IInitializable, IMoneyContainer, IEquipTarget
{
    /// <summary>
    /// 실제 인벤토리 데이터
    /// </summary>
    Inventory inventory;

    /// <summary>
    /// 아이템을 주을 수 있는 거리
    /// </summary>
    public float itemPickupRange = 2.0f;

    /// <summary>
    /// 이 인벤토리에 저장된 돈
    /// </summary>
    int money = 0;

    /// <summary>
    /// 장비 아이템의 부위별 장비 ㅓ선택
    /// </summary>
    InvenSlot[] partsSlot;

    /// <summary>
    /// 무기 장비할 트랜스폼
    /// </summary>
    Transform weaponParent;

    /// <summary>
    /// 방패 장빟ㅐ릇어
    /// </summary>
    Transform shieldParent;

    /// <summary>
    /// 인벤토리를 읽기 위한 프로퍼티
    /// </summary>
    public Inventory Inventory => inventory;

    /// <summary>
    /// 이 인벤토리에 저장된 돈에 접근하기 위한 프로퍼티
    /// </summary>
    public int Money 
    { 
        get => money;
        set
        {
            if (money != value)
            {
                money = value;
                onMoneyChange?.Invoke(money);
            }
        }
    }

    /// <summary>
    /// 장비 아이템의 부위별 상태 확인 및 설정용 인덱서
    /// </summary>
    /// <param name="part">확인할 아이템 종류</param>
    /// <returns>null이면 장비되어 있지 않음, null이 아니면 해당 슬롯에 있는 아이템이 장비되어 있음</returns>
    public InvenSlot this[EquipType part]
    {
        get => partsSlot[(int)part];
        private set
        {
            partsSlot[(int)part] = value;
        }
    }

    /// <summary>
    /// 돈에 변화가 있을 때 변화된 금액을 알리는 델리게이트(int : 최종 변경 값)
    /// </summary>
    public event Action<int> onMoneyChange;


    private void Awake()
    {
        Transform child = transform.GetChild(2);    // root
        child = child.GetChild(0);                  // pelvis
        child = child.GetChild(0);                  // spine_01
        child = child.GetChild(0);                  // spine_02
        Transform spine3 = child.GetChild(0);       // spine_03

        child = spine3.GetChild(1);                 // clavicle_l
        child = child.GetChild(1);                  // upperarm_l
        child = child.GetChild(0);                  // lowerarm_l
        child = child.GetChild(0);                  // hand_l
        shieldParent = child.GetChild(2);           // weapon_I

        child = spine3.GetChild(2);                 // clavicle_r
        child = child.GetChild(1);                  // upperarm_r
        child = child.GetChild(0);                  // lowerarm_r
        child = child.GetChild(0);                  // hand_r
        weaponParent = child.GetChild(2);           // weapon_r

        partsSlot = new InvenSlot[Enum.GetValues(typeof(EquipType)).Length];    // EquipType에 들어있는 값 종류의 개수만큼 배열 생성

    }

    

    /// <summary>
    /// 인벤토리 초기화 함수
    /// </summary>
    public void Initialize()
    {
        Player player = GetComponent<Player>();
        inventory = new Inventory(player);
        GameManager.Instance.InventoryUI.InitializeInventory(inventory);
;  }

    /// <summary>
    /// 주변 아이템을 줍는 입력이 들어왔을 때 실행될 함수
    /// </summary>
    public void GetPickupItems()
    {
        // 주변에 있는 아이템(Item레이어로 되어있다)을 모두 획득해서 인벤토리에 추가하기
        Collider[] itemColliders = Physics.OverlapSphere(transform.position, itemPickupRange, LayerMask.GetMask("Item"));
        foreach (Collider collider in itemColliders)
        {
            ItemObject item = collider.GetComponent<ItemObject>();

            if (item != null)       // null이 아니면 ItemObject라는 이야기
            {
                IConsumable consumable = item.ItemData as IConsumable;
                if (consumable != null)
                {
                    // 즉시 소비되는 아이템
                    consumable.Consume(gameObject); // 플레이어에게 즉시 사용
                    item.ItemCollected();
                }
                else
                {
                    // 인벤토리로 들어가는 아이템
                    if (Inventory.AddItem(item.ItemData.code))   // 인벤토리에 추가 시도
                    {
                        item.ItemCollected();   // 추가에 성공했으면 비활성화시켜 풀에 되돌리기
                    }
                }
            }
        }
    }

    /// <summary>
    /// 아이템을 장비하는 함수
    /// </summary>
    /// <param name="part">장비할 부위</param>
    /// <param name="slot">장비할 아이템이 들어있는 슬롯</param>
    public void EquipItem(EquipType part, InvenSlot slot)
    {
        ItemData_Equip equipItem = slot.ItemData as ItemData_Equip;
        if (equipItem != null)  // 장비가능한 아이템 일 때만 처리
        {
            Transform partParent = GetEquipParentTransform(part);
            GameObject obj = Instantiate(equipItem.equipPrefab, partParent);    // 장비 아이템 생성하고 부모에 장착
            this[part] = slot;          // 어느 파츠에 장비되었는지 기록
            slot.IsEquipped = true;     // 슬롯에 장비되었다고 표시하고 알림

            float power = 0;
            switch(part)
            {
                case EquipType.Weapon:
                    ItemData_Weapon weapon = equipItem as ItemData_Weapon;
                    power = weapon.attackPower;
                    break;

                case EquipType.Shield:
                    ItemData_Shield shield = equipItem as ItemData_Shield;
                    power = shield.defencePower;
                    break;
            }
            GameManager.Instance.Status.SetEquipPower(part, power); // 장비의 공격력과 방어력 적용
            Debug.Log($"플레이어 공격력 : {GameManager.Instance.Status.AttackPower}");
            Debug.Log($"플레이어 방어력 : {GameManager.Instance.Status.DefencePower}");
        }
    }

    /// <summary>
    /// 아이템을 장비 해제하는 함수
    /// </summary>
    /// <param name="part">장비 해제할 부위</param>
    public void UnEquipItem(EquipType part)
    {
        InvenSlot slot = partsSlot[(int)part];
        if(slot != null )       // 해제할 부위가 장비되어 있을 때만 처리
        {
            Transform partParent = GetEquipParentTransform(part);   // 부모 찾고
            while( partParent.childCount > 0)       // 부모 안에 있는 모든 자식 제거
            {
                Transform child = partParent.GetChild(0);
                child.SetParent(null);

                Destroy(child.gameObject);
            }
            slot.IsEquipped = false;        // 슬롯에 장비 해제되었다고 표시하고 알림
            this[part] = null;              // 어느 파츠가 장비 해제되었는지 기록

            GameManager.Instance.Status.SetEquipPower(part, 0);     // 장비 공격력/방어력 초기화
            Debug.Log($"플레이어 공격력 : {GameManager.Instance.Status.AttackPower}");
            Debug.Log($"플레이어 방어력 : {GameManager.Instance.Status.DefencePower}");
        }
    }

    /// <summary>
    /// 장비될 아이템이 추가될 부모 트랜스폼을 찾아주는 함수
    /// </summary>
    /// <param name="part">장비될 부위</param>
    /// <returns>장비될 부위의 부모 트랜스폼</returns>
    public Transform GetEquipParentTransform(EquipType part)
    {
        Transform result = null;

        switch(part)
        {
            case EquipType.Weapon:
                result = weaponParent;
                break;
            case EquipType.Shield:
                result = shieldParent;
                break;
        }
        return result;
    }
}
