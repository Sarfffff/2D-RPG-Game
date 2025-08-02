using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTIp
{
    [SerializeField] private TextMeshProUGUI statDescription;//项目描述文本
    public void ShowStatToolTip(string _text)  //提供显示项目名字和类型
    {

        statDescription.text = _text;//传入描述
        AdjustPosition();
        gameObject.SetActive(true);
    }
    public void HideStatToolTip()
    {
        statDescription.text = " ";
        gameObject.SetActive(false);
    }  

}