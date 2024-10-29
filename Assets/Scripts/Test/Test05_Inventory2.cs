using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test05_Inventory2 : TestBase
{
    public ItemSortCriteria sortCriteria = ItemSortCriteria.Code;
    public bool isAcending = true;

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
        base.OnTest1(context);
        inven.SlotSorting(sortCriteria, isAcending);
        inven.Test_InventoryPrint();
    }
#endif
}
