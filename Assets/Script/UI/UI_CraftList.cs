using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;  //�ϳɲ۵ĸ����壬�������ɶ�̬���ɵĺϳɲۣ������Ѿ����ڵ�װ���������Ӧ�ĸ��ӡ�
    [SerializeField] private GameObject craftSlotPrefab; //����ϳɲ۵�Ԥ���壬���ڶ�̬ʵ�����ϳɲۡ�
    [SerializeField] private List<ItemData_Equipment> craftEquipment;  //����װ�����б�


    void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetUpCraftList();
        SetupDefaultCraftWindow();

    }

    public void SetUpCraftList()
    {
        for(int i = 0;i < craftSlotParent.childCount; i++)
        {//���� craftSlots �б���������ÿ���ϳɲ۵���Ϸ����
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipment.Count; i++)//���� craftEquipment �б�
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);//Ϊÿ����������װ��ʵ����һ���µĺϳɲۡ�
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]); //����Ӧ��װ���������õ��ºϳɲ��С�
            
        }
         
    }

    public void OnPointerDown(PointerEventData eventData)  
    {
        SetUpCraftList();
    }
    public void SetupDefaultCraftWindow()//SetupDefaultCraftWindow()����ʼ������ʾ��һ��װ��������
    {
        if (craftEquipment[0]!= null)
            GetComponentInParent<UI>().craftWindow.SetCraftWindow(craftEquipment[0]);
    }

}
