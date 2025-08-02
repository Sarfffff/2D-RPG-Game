using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class InventoryItem//这个类表示库存中的物品
    //给每一个捡到的物品都创建一个独立1库存用来统计堆叠数
{

    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _newItemdata)
    {
        data = _newItemdata;
        AddStack();
    }
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
