using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;  //合成槽的父物体，用于容纳动态生成的合成槽，根据已经存在的装备生成相对应的格子。
    [SerializeField] private GameObject craftSlotPrefab; //代表合成槽的预制体，用于动态实例化合成槽。
    [SerializeField] private List<ItemData_Equipment> craftEquipment;  //制作装备的列表


    void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetUpCraftList();
        SetupDefaultCraftWindow();

    }

    public void SetUpCraftList()
    {
        for(int i = 0;i < craftSlotParent.childCount; i++)
        {//遍历 craftSlots 列表，销毁其中每个合成槽的游戏对象。
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipment.Count; i++)//遍历 craftEquipment 列表，
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);//为每个可制作的装备实例化一个新的合成槽。
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]); //将对应的装备数据设置到新合成槽中。
            
        }
         
    }

    public void OnPointerDown(PointerEventData eventData)  
    {
        SetUpCraftList();
    }
    public void SetupDefaultCraftWindow()//SetupDefaultCraftWindow()：初始化并显示第一个装备的详情
    {
        if (craftEquipment[0]!= null)
            GetComponentInParent<UI>().craftWindow.SetCraftWindow(craftEquipment[0]);
    }

}
