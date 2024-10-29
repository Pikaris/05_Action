using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerInventory), typeof(PlayerMovement), typeof(PlayerBattle))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// 인벤토리 데이터 확인용 프로퍼티(테스트용)
    /// </summary>
    public Inventory InventoryData => inventory.Inventory;

    /// <summary>
    /// 아이템 획득가능 범위 확인용 프로퍼티(아이템을 버릴 수 있는 최대 거리)
    /// </summary>
    public float ItemPickupRange => inventory.itemPickupRange;

    /// <summary>
    /// 플레이어 인벤토리에 접근하기 위한 프로퍼티
    /// </summary>
    public PlayerInventory PlayerInventory => inventory;

    // 컴포넌트들
    CharacterController characterController;
    PlayerInputController inputController;
    PlayerMovement movement;
    PlayerBattle attack;
    PlayerInventory inventory;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        inputController = GetComponent<PlayerInputController>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerBattle>();
        inventory = GetComponent<PlayerInventory>();

        inputController.onMove += movement.SetDirection;
        inputController.onMoveModeChange += movement.ToggleMoveMode;
        inputController.onAttack += attack.OnAttackInput;
        inputController.onPickUp += inventory.GetPickupItems;
    }

    public void Initialize()
    {
        IInitializable[] inits = GetComponents<IInitializable>();
        foreach (var init in inits)
        {
            init.Initialize();
        }
    }
}
