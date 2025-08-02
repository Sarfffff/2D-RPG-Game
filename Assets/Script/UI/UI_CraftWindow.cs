using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName; //װ������
    [SerializeField] private TextMeshProUGUI itemDescription;//װ������
    [SerializeField] private Image itemIcon; //װ��ͼ��
    [SerializeField] private Button craftButton;  //�ϳɰ�ť

    [SerializeField] private Image[] materialImage;  //�ϳ���������б�


    public void SetCraftWindow(ItemData_Equipment _data)   
    {
        
        craftButton.onClick.RemoveAllListeners();
        for (int i = 0; i < materialImage.Length; i++)//������ղ��ϵ�ͼ���Լ�ͼ�����½ǵ������������ַ�
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        //����ǲ���
        for (int i = 0; i < _data.craftingMaterials.Count; i++)  //�����ϳ�װ��������Ĳ���
        {
            if (_data.craftingMaterials.Count > materialImage.Length)
                Debug.LogWarning("��ӵ�еĲ��ϱȺϳ���Ҫ�Ĳ��϶�");

            //��ÿ�����ϵ�ͼ�����õ���Ӧ�� materialImage �У�����ͼ����ɫ����Ϊ��ɫ����ʾͼ�ꡣ
            materialImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();
            //��ȡÿ��ͼ���ӽڵ��е� TextMeshProUGUI ����������ı�����Ϊ������ϵ�������������ɫ����Ϊ��ɫ����ʾ�ı���


            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().text = _data.craftingMaterials[i].stackSize.ToString();
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        }

        //�ϳɵ�װ����ͼ��
        itemIcon.sprite = _data.icon;
        itemName.text = _data.ItemName;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.Instance.CanCraft(_data, _data.craftingMaterials));
    }
}
