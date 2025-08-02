using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Serializable_Dictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{ //序列化字典，将字典类型变成可以保存和传输的状态
    [SerializeField] private List<TKey> keys = new List<TKey>();  //存储字典键的列表
    [SerializeField] private List<TValue> values = new List<TValue>();  //存储字典值的列表



    public void OnBeforeSerialize()  //在Unity序列化对象之前自动调用
    {
        keys.Clear();
        values.Clear();
        //清空字典所有的键值对


        foreach (KeyValuePair<TKey, TValue> kvp in this)  //将字典中的键值对分别添加到对应的列表中
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
    public void OnAfterDeserialize() //在Unity反序列化对象之后自动调用
    {
        this.Clear();
        // 清空当前字典内容

        //检查键值列表长度是否一致
        if (keys.Count != values.Count)
        {
            Debug.Log("键数和值数不相等");
            return;
        }

        //将列表中的数据重新构建为字典
        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }

}
