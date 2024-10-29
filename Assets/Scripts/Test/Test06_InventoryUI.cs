using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test06_InventoryUI : TestBase
{
    public InventoryUI inventoryUI;
    Inventory inven;

    public ItemCode code = ItemCode.Misc;
    [Range(0, 5)]
    public uint from = 0;
    [Range(0, 5)]
    public uint to = 0;

#if UNITY_EDITOR
    private void Start()
    {
        inven = new Inventory(null);
        inven.AddItem(ItemCode.Ruby);
        inven.AddItem(ItemCode.Sapphire);
        inven.AddItem(ItemCode.Sapphire);
        inven.AddItem(ItemCode.Sapphire);
        inven.AddItem(ItemCode.Sapphire,2);
        inven.AddItem(ItemCode.Emerald, 3);
        inven.AddItem(ItemCode.Emerald, 5);
        //inven.MoveItem(2, 3);
        inven.AddItem(ItemCode.Emerald, 5);
        inven.AddItem(ItemCode.Emerald, 4);
        inven.Test_InventoryPrint();

        inventoryUI.InitializeInventory(inven);
        inventoryUI.Test_Open();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        inven.AddItem(code, from);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        inven.RemoveItem(from);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        inven.ClearInventory();
    }
#endif
}
