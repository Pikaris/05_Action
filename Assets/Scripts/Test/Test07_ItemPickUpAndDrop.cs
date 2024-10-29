using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test07_ItemPickUpAndDrop  : TestBase
{
    public ItemCode code = ItemCode.Misc;
    public uint spawnCount = 1;
    public Transform spawnPosition;
    public bool noise = false;

    protected Player player;

#if UNITY_EDITOR
    protected virtual void Start()
    {
        spawnPosition = transform.GetChild(0);

        player = GameManager.Instance.Player;
        player.InventoryData.AddItem(ItemCode.Ruby);
        player.InventoryData.AddItem(ItemCode.Ruby);
        player.InventoryData.AddItem(ItemCode.Sapphire);
        player.InventoryData.AddItem(ItemCode.Sapphire);
        player.InventoryData.AddItem(ItemCode.Sapphire);
        player.InventoryData.AddItem(ItemCode.Sapphire, 2);
        player.InventoryData.AddItem(ItemCode.Emerald, 3);
        player.InventoryData.AddItem(ItemCode.Emerald, 5);
        player.InventoryData.MoveItem(2, 3);
        player.InventoryData.AddItem(ItemCode.Emerald, 5);
        player.InventoryData.AddItem(ItemCode.Emerald, 4);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.MakeItems(code, spawnCount, spawnPosition.position, noise);
    }
#endif
}
