using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName; //装备名称
    [SerializeField] private TextMeshProUGUI itemDescription;//装备描述
    [SerializeField] private Image itemIcon; //装备图像
    [SerializeField] private Button craftButton;  //合成按钮

    [SerializeField] private Image[] materialImage;  //合成所需材料列表


    public void SetCraftWindow(ItemData_Equipment _data)   
    {
        
        craftButton.onClick.RemoveAllListeners();
        for (int i = 0; i < materialImage.Length; i++)//首先清空材料的图像，以及图像右下角的所需数量的字符
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        //这个是材料
        for (int i = 0; i < _data.craftingMaterials.Count; i++)  //遍历合成装备的所需的材料
        {
            if (_data.craftingMaterials.Count > materialImage.Length)
                Debug.LogWarning("你拥有的材料比合成需要的材料多");

            //将每个材料的图标设置到对应的 materialImage 中，并将图像颜色设置为白色以显示图标。
            materialImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();
            //获取每个图像孩子节点中的 TextMeshProUGUI 组件，将其文本设置为所需材料的数量，并将颜色设置为白色以显示文本。


            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().text = _data.craftingMaterials[i].stackSize.ToString();
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        }

        //合成的装备的图像
        itemIcon.sprite = _data.icon;
        itemName.text = _data.ItemName;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.Instance.CanCraft(_data, _data.craftingMaterials));
    }
}
