using System.Diagnostics;
using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler//�ӿ�
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
    {//�˷������Ƚ���Ʒ������Ʒ����Ϊ����Ʒ��Ȼ����Ʒͼ�����ɫ����Ϊ��ɫ��
     //�������Ʒ��Ϊ�գ�����������Ʒͼ��Ϊ��Ʒ��ͼ�꣬��������Ʒ�����������ı���ʾ�������Ʒ��������1����ʾ���������򣬲���ʾ�κ��ı���
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
        if (Input.GetKey(KeyCode.LeftControl))  //��ctrlɾ����Ʒ
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
