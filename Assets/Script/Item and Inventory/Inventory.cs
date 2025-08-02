
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISavedManager  //单例模式 实现库存管理的单例模式，负责物品的添加和移除操作
{
    public static Inventory Instance;

    public List<ItemData> startingItems;//初始装备 

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;//字典 键值对

    public List<InventoryItem> inventory;//仓库列表
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;//字典 键值对

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;//字典 键值对


    [Header("仓库")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("物品冷却时间")]
    private float lastTimeUserFlask ;
    private float lastTimeUsedArmor ;
    public float flaskCooldown { get; private set; }
    public float armorCooldown {  get; private set; }

    [Header("数据存储")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;//加载的物品
    public List<ItemData_Equipment> loadedEquipment;
    private void Awake()//只允许存在一个
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }
    private void Start()
    {

        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();

    }

    private void AddStartingItems()
    {

        foreach (ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);

        }


        if (loadedItems.Count > 0)  //将保存的物品进行更新到库存中
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }

            return;
        }
        for (int i = 0; i < startingItems.Count; i++)
        {
            if(startingItems[i] != null)
                AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;  //类似强制类型转换
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }
        if(oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);

        RemoveItem(_item);
        newEquipment.AddModifiers();
        UpdateSlotUI();
    }

    public  void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {


            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
            UpdateSlotUI();
        }

    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {

            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }

        }
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
        UpdateStatsUI();
    }

    public  void UpdateStatsUI()
    {
        for (int i = 0; i < statSlot.Length; i++)  //更新UI界面中的面板数值
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public bool CanAddItem()  //检查仓库是否有空位，inventory 的数量和 inventoryItemSlot 的长度
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            return false;
        }
        return true;
    }
    public void AddItem(ItemData _item)  //往列表添加物品
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(_item);

        }
        else if(_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }

        UpdateSlotUI();
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))//如果物品已经存在，增加堆叠数
        {
            value.AddStack();
        }
        else  //不存在，新建仓库将其添加进去
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }
    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))//如果物品已经存在，增加堆叠数
        {
            value.AddStack();
        }
        else  //不存在，新建仓库将其添加进去
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item) { 
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))//如果物品的堆叠数量小于等于 1，则从列表和字典中移除该物品
        {
            if(value.stackSize <=1 )
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {//否则，减少其堆叠数量。
                value.RemoveStack();
            }
        }
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashvalue))//如果物品的堆叠数量小于等于 1，则从列表和字典中移除该物品
        {
            if (stashvalue.stackSize <= 1)
            {
                stash.Remove(stashvalue);
                stashDictionary.Remove(_item);
            }
            else
            {//否则，减少其堆叠数量。
                stashvalue.RemoveStack();
            }
        }
        UpdateSlotUI();
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft,List<InventoryItem> _requireMaterials)
    {
        List<InventoryItem> _materialsToRemove = new List<InventoryItem>();
        for (int i = 0; i < _requireMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requireMaterials[i].data, out InventoryItem stashValue))
            {
                if(stashValue.stackSize < _requireMaterials[i].stackSize)
                {
                    return false;
                }
                else
                {
                    _materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                return false;
            }
        }
        for(int i =0;i< _materialsToRemove.Count; i++)
        {
            RemoveItem(_materialsToRemove[i].data);
        }
        AddItem(_itemToCraft);
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _type)  //得到装备的类型
    {
        ItemData_Equipment equipedItem= null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)//遍历字典，找到与传入的装备类型相匹配的，最后返回
        {
            if (item.Key.equipmentType == _type)  
            {
                equipedItem = item.Key;
            }
        }
        return equipedItem;
    }

    public void UsedFlask()
    {
        
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);
        if (currentFlask == null)
            return;

        bool canUsedFlask = Time.time > lastTimeUserFlask + flaskCooldown;
        if (canUsedFlask)
        {
            flaskCooldown = currentFlask.ItemCooldown;
            currentFlask.Effect(null);
            lastTimeUserFlask = Time.time;

        }
        else
        {
            Debug.Log("药品冷却中");
        }
    }
    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.armor);
        if(Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.ItemCooldown;
            lastTimeUsedArmor = Time.time;  
            return true;
        }
        return false;
    }
    private void Update()
    {
       
    }

    public void LoadData(GameData gameData)//从 GameData 对象里加载物品数据到 loadedItems 列表。
    {
        foreach (KeyValuePair<string, int> pair in gameData.inventory)//如果Ganedata对象的物品的Id与物品数据库中的Id一样，则加载到loadedItems中进行外部显示
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }
        foreach (string loadedItemId in gameData.equipmentId)//如果Ganedata对象的物品的Id与物品数据库中装备的Id一样，则加载到loadedItems中进行外部显示
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemId == loadedItemId)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }

    }

    public void SaveData(ref GameData gameData)
    {
        gameData.inventory.Clear();
        gameData.equipmentId.Clear();
        foreach(KeyValuePair<ItemData,InventoryItem> pair in inventoryDictionary)
        {
            gameData.inventory.Add(pair.Key.itemId,pair.Value.stackSize);  //遍历inventory库存，将库存中的每个物品的Id以及堆叠数存入gamedata类中的字典进行保存
        }
        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            gameData.inventory.Add(pair.Key.itemId, pair.Value.stackSize);  //遍历stashDictionary库存，将库存中的每个物品的Id以及堆叠数存入gamedata类中的字典进行保存
        }
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            gameData.equipmentId.Add(pair.Key.itemId);  //遍历stashDictionary库存，将库存中的每个物品的Id以及堆叠数存入gamedata类中的字典进行保存
        }
    }
#if UNITY_EDITOR
    [ContextMenu("填充物品数据库")]
    private void FillUpItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());
    private List<ItemData> GetItemDataBase()//获取物品数据库
    {
        List<ItemData> itemDatabase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] {"Assets/Data/Items"});//查找该路径下的资产

        foreach (string SOName in assetNames)   //遍历资产
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);//把 GUID 转换为资产路径。
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);  //依据资产路径加载ItemData对象
            itemDatabase.Add(itemData);  //将其添加到列表中
        }

        return itemDatabase;
    }
#endif
}
