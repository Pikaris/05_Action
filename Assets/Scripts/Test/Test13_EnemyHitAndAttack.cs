using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test13_EnemyHitAndAttack : TestBase
{
    public int damage = 20;
    public Enemy enemy;
    public EnemyHealth health;
    public EnemyBattle battle;


    private void Start()
    {
        battle = health.GetComponent<EnemyBattle>();
        battle.onHit += (final) => Debug.Log($"{final} 데미지를 입음");

        Player player = GameManager.Instance.Player;
        player.InventoryData.AddItem(ItemCode.IronSword);   // 칼 추가
        player.PlayerInventory.EquipItem(EquipType.Weapon, player.InventoryData[0]);    // 추가한 칼 장비

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        movement.ToggleMoveMode();  // 걷기 모드로 변경 
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        health.GetDamage(damage);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        battle.Defense(damage);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        health.HealthHeal(100);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        enemy.DropItems();
    }
}
