using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTIp
{
    [SerializeField] private TextMeshProUGUI statDescription;//��Ŀ�����ı�
    public void ShowStatToolTip(string _text)  //�ṩ��ʾ��Ŀ���ֺ�����
    {

        statDescription.text = _text;//��������
        AdjustPosition();
        gameObject.SetActive(true);
    }
    public void HideStatToolTip()
    {
        statDescription.text = " ";
        gameObject.SetActive(false);
    }  

}