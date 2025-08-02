using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTreeToolTip : UI_ToolTIp
{
    [SerializeField] private TextMeshProUGUI skillDescription;//��Ŀ�����ı�
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillcost;
    public void ShowSkillToolTip(string _text,string _name,float _price)  //�ṩ��ʾ��Ŀ���ֺ�����
    {


        skillDescription.text = _text;//��������
        skillName.text = _name;
        skillcost.text = "�۸� : " + _price;
        //�޸������С����ֹ�������
        //if(itemDescription.text.Length > 12)
        //    itemNameText.fontSize = itemNameText.fontSize * .7f;
        //else
        //    itemNameText.fontSize = defaultFontSize;

        AdjustPosition();
        gameObject.SetActive(true);

    }
    public void HideSkillToolTip() => gameObject.SetActive(false);
}
