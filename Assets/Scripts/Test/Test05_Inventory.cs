using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test05_Inventory : TestBase
{
    public ItemCode code = ItemCode.Misc;
    [Range(0, 5)]
    public uint from = 0;
    [Range(0, 5)]
    public uint to = 0;

    Inventory inven;
#if UNITY_EDITOR
    private void Start()
    {
        inven = new Inventory(null);
        inven.AddItem(ItemCode.Ruby);
        inven.AddItem(ItemCode.Sapphire);
        inven.AddItem(ItemCode.Sapphire);
        inven.AddItem(ItemCode.Emerald);
        inven.AddItem(ItemCode.Emerald);
        inven.AddItem(ItemCode.Emerald);
        inven.MoveItem(2, 3);
        inven.Test_InventoryPrint();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        inven.AddItem(code, from);
        inven.Test_InventoryPrint();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        inven.RemoveItem(from);
        inven.Test_InventoryPrint();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        inven.ClearSlot(from);
        inven.Test_InventoryPrint();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        inven.ClearInventory();
        inven.Test_InventoryPrint();
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        inven.MoveItem(from, to);
        inven.Test_InventoryPrint();
    }

    protected override void OnTestLClick(InputAction.CallbackContext context)
    {
    }

#endif
}
