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
        // ����ж��װ��
        TryUnequipItem();
    }

    private void TryUnequipItem()
    {
        // ��� item �Ƿ�Ϊ��
        if (item == null || item.data == null)
        {
            return;
        }

        // ���Խ� item.data ת��Ϊ ItemData_Equipment
        ItemData_Equipment equipment = item.data as ItemData_Equipment;
        if (equipment == null)
        {
            return;
        }

        try
        {
            Inventory.Instance.AddItem(equipment);
            // ж��װ��
            Inventory.Instance.UnequipItem(equipment);
            ui.itemToolTip.HideToolTip();
           
            // ����װ����
            CleanUpSlot();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Unequipping item failed: {e.Message}");
        }
    }
}