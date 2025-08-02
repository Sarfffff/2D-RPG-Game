using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("人物掉落")]
    [SerializeField] private float chanceToLooseItems;  //掉落Item的几率
    [SerializeField] private float chanceToLoosematerials;  //掉落材料的几率

    public override void GenerateDrop()
    {

        Inventory inventory = Inventory.Instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToLoose = new List<InventoryItem>();



        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.data);

                itemsToUnequip.Add(item);

                //inventory.UnequipItem(item.data as ItemData_Equipment); 

            }

        }
        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.data);
                materialsToLoose.Add(item);

            }
        }
        for (int i = 0; i < materialsToLoose.Count; i++)
        {
            inventory.RemoveItem(materialsToLoose[i].data);

        }
    }
}
