
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISavedManager  //����ģʽ ʵ�ֿ�����ĵ���ģʽ��������Ʒ����Ӻ��Ƴ�����
{
    public static Inventory Instance;

    public List<ItemData> startingItems;//��ʼװ�� 

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;//�ֵ� ��ֵ��

    public List<InventoryItem> inventory;//�ֿ��б�
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;//�ֵ� ��ֵ��

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;//�ֵ� ��ֵ��


    [Header("�ֿ�")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("��Ʒ��ȴʱ��")]
    private float lastTimeUserFlask ;
    private float lastTimeUsedArmor ;
    public float flaskCooldown { get; private set; }
    public float armorCooldown {  get; private set; }

    [Header("���ݴ洢")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;//���ص���Ʒ
    public List<ItemData_Equipment> loadedEquipment;
    private void Awake()//ֻ�������һ��
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


        if (loadedItems.Count > 0)  //���������Ʒ���и��µ������
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
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;  //����ǿ������ת��
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
        for (int i = 0; i < statSlot.Length; i++)  //����UI�����е������ֵ
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public bool CanAddItem()  //���ֿ��Ƿ��п�λ��inventory �������� inventoryItemSlot �ĳ���
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            return false;
        }
        return true;
    }
    public void AddItem(ItemData _item)  //���б������Ʒ
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
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))//�����Ʒ�Ѿ����ڣ����Ӷѵ���
        {
            value.AddStack();
        }
        else  //�����ڣ��½��ֿ⽫����ӽ�ȥ
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }
    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))//�����Ʒ�Ѿ����ڣ����Ӷѵ���
        {
            value.AddStack();
        }
        else  //�����ڣ��½��ֿ⽫����ӽ�ȥ
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item) { 
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))//�����Ʒ�Ķѵ�����С�ڵ��� 1������б���ֵ����Ƴ�����Ʒ
        {
            if(value.stackSize <=1 )
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {//���򣬼�����ѵ�������
                value.RemoveStack();
            }
        }
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashvalue))//�����Ʒ�Ķѵ�����С�ڵ��� 1������б���ֵ����Ƴ�����Ʒ
        {
            if (stashvalue.stackSize <= 1)
            {
                stash.Remove(stashvalue);
                stashDictionary.Remove(_item);
            }
            else
            {//���򣬼�����ѵ�������
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

    public ItemData_Equipment GetEquipment(EquipmentType _type)  //�õ�װ��������
    {
        ItemData_Equipment equipedItem= null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)//�����ֵ䣬�ҵ��봫���װ��������ƥ��ģ���󷵻�
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
            Debug.Log("ҩƷ��ȴ��");
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

    public void LoadData(GameData gameData)//�� GameData �����������Ʒ���ݵ� loadedItems �б�
    {
        foreach (KeyValuePair<string, int> pair in gameData.inventory)//���Ganedata�������Ʒ��Id����Ʒ���ݿ��е�Idһ��������ص�loadedItems�н����ⲿ��ʾ
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
        foreach (string loadedItemId in gameData.equipmentId)//���Ganedata�������Ʒ��Id����Ʒ���ݿ���װ����Idһ��������ص�loadedItems�н����ⲿ��ʾ
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
            gameData.inventory.Add(pair.Key.itemId,pair.Value.stackSize);  //����inventory��棬������е�ÿ����Ʒ��Id�Լ��ѵ�������gamedata���е��ֵ���б���
        }
        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            gameData.inventory.Add(pair.Key.itemId, pair.Value.stackSize);  //����stashDictionary��棬������е�ÿ����Ʒ��Id�Լ��ѵ�������gamedata���е��ֵ���б���
        }
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            gameData.equipmentId.Add(pair.Key.itemId);  //����stashDictionary��棬������е�ÿ����Ʒ��Id�Լ��ѵ�������gamedata���е��ֵ���б���
        }
    }
#if UNITY_EDITOR
    [ContextMenu("�����Ʒ���ݿ�")]
    private void FillUpItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());
    private List<ItemData> GetItemDataBase()//��ȡ��Ʒ���ݿ�
    {
        List<ItemData> itemDatabase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] {"Assets/Data/Items"});//���Ҹ�·���µ��ʲ�

        foreach (string SOName in assetNames)   //�����ʲ�
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);//�� GUID ת��Ϊ�ʲ�·����
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);  //�����ʲ�·������ItemData����
            itemDatabase.Add(itemData);  //������ӵ��б���
        }

        return itemDatabase;
    }
#endif
}
