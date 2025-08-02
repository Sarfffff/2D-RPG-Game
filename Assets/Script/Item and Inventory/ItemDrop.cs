using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;//可能的掉落物品，暴露在外部，手动装入
    private List<ItemData> dropList = new List<ItemData>();  //将掉落物品填入掉落列表


    [SerializeField] private GameObject dropPrefab;

    public  virtual void GenerateDrop()
    {
        // 遍历所有可能的掉落物品
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        // 检查掉落列表是否为空
        if (dropList.Count == 0)
        {
            Debug.Log("No items to drop.");
            return;
        }

        // 生成指定数量的掉落物品
        for (int i = 0; i < possibleItemDrop; i++)
        {
            if (dropList.Count == 0)
            {
                Debug.Log("Not enough items to drop.");
                break;
            }

            // 随机选择一个物品
            ItemData randomItem = dropList[Random.Range(0, dropList.Count-1)];
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }
    protected void  DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab,transform.position,Quaternion.identity );  //掉落物品方法，生成掉落预制体

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));   //掉落时有掉落速度
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);   //传入速度和物品
    }
}
