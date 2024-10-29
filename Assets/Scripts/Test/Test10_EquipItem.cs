using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test10_EquipItem : Test07_ItemPickUpAndDrop
{
#if UNITY_EDITOR
    protected override void Start()
    {
        spawnPosition = transform.GetChild(0);

        player = GameManager.Instance.Player;
        player.InventoryData.AddItem(ItemCode.IronSword);
        player.InventoryData.AddItem(ItemCode.SilverSword);
        player.InventoryData.AddItem(ItemCode.OldSword);
        player.InventoryData.AddItem(ItemCode.KiteShield);
        player.InventoryData.AddItem(ItemCode.RoundShield);
    }
#endif
}
