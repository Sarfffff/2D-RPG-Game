using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour//添加物品类
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private SpriteRenderer sr;

    private void SetUpVisual()
    {
        if (itemData == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object -" + itemData.name;
    }


    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;

        rb.velocity = _velocity;
        SetUpVisual();
    }
    public void PickUpItem()
    {
        if (!Inventory.Instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0,7);
            AudioManager.instance.PlaySFX(18,transform);
            PlayerManager.instance.player.playerFx.CreatePopUpText("没有足够的空间");
            return;
        }
        Inventory.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
