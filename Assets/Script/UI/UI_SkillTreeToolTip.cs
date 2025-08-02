using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTreeToolTip : UI_ToolTIp
{
    [SerializeField] private TextMeshProUGUI skillDescription;//项目描述文本
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillcost;
    public void ShowSkillToolTip(string _text,string _name,float _price)  //提供显示项目名字和类型
    {


        skillDescription.text = _text;//传入描述
        skillName.text = _name;
        skillcost.text = "价格 : " + _price;
        //修改字体大小，防止字体过长
        //if(itemDescription.text.Length > 12)
        //    itemNameText.fontSize = itemNameText.fontSize * .7f;
        //else
        //    itemNameText.fontSize = defaultFontSize;

        AdjustPosition();
        gameObject.SetActive(true);

    }
    public void HideSkillToolTip() => gameObject.SetActive(false);
}
