using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISavedManager
{
    public static PlayerManager instance;  //����ģʽ��һ����ֻ��һ��ʵ�����Ķ��󣬲����ṩȫ�ֵķ��ʵ�,ͨ�� PlayerManager.instance ȫ�ַ���
    public Player player;

    public int currency;  //������money
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    public bool HaveEnoughMOney(int price) //�Ƿ�����㹻��Soul������
    {
        if(currency < price)
        {
            return false;
        }
        currency = currency - price;
        return true;
    }
    public int GetCurrency() => currency;

    public void LoadData(GameData _data)  //���ر��������
    {
        this.currency = _data.currency;  //�������
    }

    public void SaveData(ref GameData _data)  //��������
    {
        _data.currency =this.currency;  //�������
    }
}
