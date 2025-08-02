using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class InventoryItem//������ʾ����е���Ʒ
    //��ÿһ���񵽵���Ʒ������һ������1�������ͳ�ƶѵ���
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
