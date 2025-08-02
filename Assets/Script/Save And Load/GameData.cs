using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] //����������Ա����л���Ҳ���ǿ��Խ���Ķ���ת��Ϊ�ֽ������Ա㱣�浽�ļ��С�
public class GameData//������Ϸ���ݵĽṹ
{
    public int currency;  //��Ϸ���ҡ�
    public Serializable_Dictionary<string, int> inventory;//������  unityĬ�ϲ�֧���ֵ����͵����л�Ҳ���Ǵ洢�ʹ��䣬������Ҫдһ���ű����л��ֵ䣬����ʹ��2���б�
    public Serializable_Dictionary<string, bool> skillTree;
    public List<string> equipmentId;
    public Serializable_Dictionary<string, bool> checkpoints;
    public string closetCheckPointId;
    public Serializable_Dictionary<string, float> volumeSettings;
    //���
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
