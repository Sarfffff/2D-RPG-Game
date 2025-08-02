using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;//���ܵĵ�����Ʒ����¶���ⲿ���ֶ�װ��
    private List<ItemData> dropList = new List<ItemData>();  //��������Ʒ��������б�


    [SerializeField] private GameObject dropPrefab;

    public  virtual void GenerateDrop()
    {
        // �������п��ܵĵ�����Ʒ
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        // �������б��Ƿ�Ϊ��
        if (dropList.Count == 0)
        {
            Debug.Log("No items to drop.");
            return;
        }

        // ����ָ�������ĵ�����Ʒ
        for (int i = 0; i < possibleItemDrop; i++)
        {
            if (dropList.Count == 0)
            {
                Debug.Log("Not enough items to drop.");
                break;
            }

            // ���ѡ��һ����Ʒ
            ItemData randomItem = dropList[Random.Range(0, dropList.Count-1)];
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }
    protected void  DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab,transform.position,Quaternion.identity );  //������Ʒ���������ɵ���Ԥ����

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));   //����ʱ�е����ٶ�
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);   //�����ٶȺ���Ʒ
    }
}
