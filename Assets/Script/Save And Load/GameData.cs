using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] //表明该类可以被序列化，也就是可以将类的对象转换为字节流，以便保存到文件中。
public class GameData//定义游戏数据的结构
{
    public int currency;  //游戏货币‘
    public Serializable_Dictionary<string, int> inventory;//保存库存  unity默认不支持字典类型的序列化也就是存储和传输，所以需要写一个脚本序列化字典，或者使用2个列表
    public Serializable_Dictionary<string, bool> skillTree;
    public List<string> equipmentId;
    public Serializable_Dictionary<string, bool> checkpoints;
    public string closetCheckPointId;
    public Serializable_Dictionary<string, float> volumeSettings;
    //灵魂
    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public GameData()
    {
        this.currency = 0;
        inventory = new Serializable_Dictionary<string, int>();
        skillTree = new Serializable_Dictionary<string, bool>();
        equipmentId = new List<string>();

        closetCheckPointId = string.Empty;
        checkpoints = new Serializable_Dictionary<string,bool>();
        volumeSettings = new Serializable_Dictionary<string, float>();


        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;
    }
}
