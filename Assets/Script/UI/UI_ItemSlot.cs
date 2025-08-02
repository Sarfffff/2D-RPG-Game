using System.Diagnostics;
using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler//接口
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    protected virtual void Start()
    {
      ui = GetComponentInParent<UI>();
    }
    public void UpdateSlot(InventoryItem _newItem)
    {//此方法首先将物品栏的物品设置为新物品，然后将物品图像的颜色设置为白色。
     //如果新物品不为空，它会设置物品图像为物品的图标，并根据物品的数量更新文本显示。如果物品数量大于1，显示数量；否则，不显示任何文本。
        item = _newItem;
        itemImage.color = Color.white;
        if (item != null)
        {

            itemImage.sprite = item.data.icon;
            if (item.stackSize > 0)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }
    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;
        if (Input.GetKey(KeyCode.LeftControl))  //左ctrl删除物品
        {
            Inventory.Instance.RemoveItem(item.data);
            return;
        }
            

       
        if (item.data.itemType == ItemType.Equipment)
            Inventory.Instance.EquipItem(item.data);

        ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null) 
            return;
        Vector2 mousePosition = Input.mousePosition;
        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)
            xOffset = -150;
        else
            xOffset = 150;

        if (mousePosition.y > 320)
            yOffset = -150;
        else
            yOffset = 150;

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
        ui.itemToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) 
            return;

        ui.itemToolTip.HideToolTip();
    }
}
