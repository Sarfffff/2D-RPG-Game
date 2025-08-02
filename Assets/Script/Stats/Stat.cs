using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//��װ����stat������
public class Stat
{
    [SerializeField]  private int baseValue;//��������
    public List<int> modifiers;//�洢�Ի�����ֵ�����η�����Щ���η������������ӻ��߼��ٻ�����ֵ��
    public int GetValue()
    {
        int finalValue = baseValue;
        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        return finalValue;
    }
    public void SetDefaultValue(int _value) //
    {
        baseValue = _value;
    }
    public  void AddModifers(int _modifiers)  //���б����ֵ���������б�õ�����ֵ
    {
        modifiers.Add(_modifiers);
    }
    public void RemoveModifers(int _modifiers)//���б�ɾ��ֵ���������б�õ�����ֵ
    {
        modifiers.Remove(_modifiers);
    }
}
