using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//封装带有stat的属性
public class Stat
{
    [SerializeField]  private int baseValue;//基础属性
    public List<int> modifiers;//存储对基础数值的修饰符。这些修饰符可以用来增加或者减少基础数值。
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
    public  void AddModifers(int _modifiers)  //往列表添加值，最后遍历列表得到最终值
    {
        modifiers.Add(_modifiers);
    }
    public void RemoveModifers(int _modifiers)//往列表删除值，最后遍历列表得到最终值
    {
        modifiers.Remove(_modifiers);
    }
}
