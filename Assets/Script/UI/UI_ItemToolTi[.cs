using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;//项目描述文本
    void Start()
    {
        
    }
    public void ShowToolTip(ItemData_Equipment item)  //提供显示项目名字和类型
    {
        if(item == null)
            return;
        itemNameText.text = item.ItemName;  //传入名字
        itemTypeText.text = item.equipmentType.ToString();//传入类型
        itemDescription.text = item.GetDescription();//传入描述


        //修改字体大小，防止字体过长
        //if(itemDescription.text.Length > 12)
        //    itemNameText.fontSize = itemNameText.fontSize * .7f;
        //else
        //    itemNameText.fontSize = defaultFontSize;


        gameObject.SetActive(true);

    }
    public void HideToolTip()=>gameObject.SetActive(false);
   
}
