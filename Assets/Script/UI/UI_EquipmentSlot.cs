using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // 尝试卸下装备
        TryUnequipItem();
    }

    private void TryUnequipItem()
    {
        // 检查 item 是否为空
        if (item == null || item.data == null)
        {
            return;
        }

        // 尝试将 item.data 转换为 ItemData_Equipment
        ItemData_Equipment equipment = item.data as ItemData_Equipment;
        if (equipment == null)
        {
            return;
        }

        try
        {
            Inventory.Instance.AddItem(equipment);
            // 卸下装备
            Inventory.Instance.UnequipItem(equipment);
            ui.itemToolTip.HideToolTip();
           
            // 清理装备槽
            CleanUpSlot();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Unequipping item failed: {e.Message}");
        }
    }
}