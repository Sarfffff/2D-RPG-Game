using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISavedManager
{
    public static PlayerManager instance;  //单例模式，一个类只有一个实例化的对象，并且提供全局的访问点,通过 PlayerManager.instance 全局访问
    public Player player;

    public int currency;  //类似于money
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    public bool HaveEnoughMOney(int price) //是否存在足够的Soul购买技能
    {
        if(currency < price)
        {
            return false;
        }
        currency = currency - price;
        return true;
    }
    public int GetCurrency() => currency;

    public void LoadData(GameData _data)  //加载保存的数据
    {
        this.currency = _data.currency;  //加载灵魂
    }

    public void SaveData(ref GameData _data)  //保存数据
    {
        _data.currency =this.currency;  //保存灵魂
    }
}
