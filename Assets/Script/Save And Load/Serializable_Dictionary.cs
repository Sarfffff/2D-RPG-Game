using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Serializable_Dictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{ //���л��ֵ䣬���ֵ����ͱ�ɿ��Ա���ʹ����״̬
    [SerializeField] private List<TKey> keys = new List<TKey>();  //�洢�ֵ�����б�
    [SerializeField] private List<TValue> values = new List<TValue>();  //�洢�ֵ�ֵ���б�



    public void OnBeforeSerialize()  //��Unity���л�����֮ǰ�Զ�����
    {
        keys.Clear();
        values.Clear();
        //����ֵ����еļ�ֵ��


        foreach (KeyValuePair<TKey, TValue> kvp in this)  //���ֵ��еļ�ֵ�Էֱ���ӵ���Ӧ���б���
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
    public void OnAfterDeserialize() //��Unity�����л�����֮���Զ�����
    {
        this.Clear();
        // ��յ�ǰ�ֵ�����

        //����ֵ�б����Ƿ�һ��
        if (keys.Count != values.Count)
        {
            Debug.Log("������ֵ�������");
            return;
        }

        //���б��е��������¹���Ϊ�ֵ�
        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }

}
