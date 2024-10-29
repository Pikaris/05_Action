using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemDataManager))]
public class GameManager : Singleton<GameManager>
{
    Player player;
    PlayerStatus status;

    InventoryUI inventoryUI;
    ItemDataManager itemDataManager;

    /// <summary>
    /// 플레이어 접근용 프로퍼티
    /// </summary>
    public Player Player => player;

    /// <summary>
    /// 플레이어 스테이터스 접근용 프로퍼티
    /// </summary>
    public PlayerStatus Status => status;

    /// <summary>
    /// ItemDataManager. 인덱서로 개별 아이템 데이터에 접근 가능
    /// </summary>
    public ItemDataManager ItemData => itemDataManager;

    /// <summary>
    /// 인벤토리 UI 접근용 프로퍼티(Get만 가능)
    /// </summary>
    public InventoryUI InventoryUI => inventoryUI;


    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        itemDataManager = GetComponent<ItemDataManager>();
    }
    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindAnyObjectByType<Player>();
        status = player.GetComponent<PlayerStatus>();
        inventoryUI = FindAnyObjectByType<InventoryUI>();

        player?.Initialize();
    }
}
