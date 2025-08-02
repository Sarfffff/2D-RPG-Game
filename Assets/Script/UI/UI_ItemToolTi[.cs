using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;//��Ŀ�����ı�
    void Start()
    {
        
    }
    public void ShowToolTip(ItemData_Equipment item)  //�ṩ��ʾ��Ŀ���ֺ�����
    {
        if(item == null)
            return;
        itemNameText.text = item.ItemName;  //��������
        itemTypeText.text = item.equipmentType.ToString();//��������
        itemDescription.text = item.GetDescription();//��������


        //�޸������С����ֹ�������
        //if(itemDescription.text.Length > 12)
        //    itemNameText.fontSize = itemNameText.fontSize * .7f;
        //else
        //    itemNameText.fontSize = defaultFontSize;


        gameObject.SetActive(true);

    }
    public void HideToolTip()=>gameObject.SetActive(false);
   
}
