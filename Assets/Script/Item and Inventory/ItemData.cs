
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public enum ItemType
{
    Material,
    Equipment
}
[CreateAssetMenu(fileName = "New Item data", menuName = "Data/Item")]
public class ItemData : ScriptableObject //һ�������洢�̶����ݣ�������Ʒ�����ݽṹ
{
    public ItemType itemType;
    public string ItemName;
    public Sprite icon; 
    public string itemId;

    [Range(0,100)]
    public int dropChance;
    protected StringBuilder sb= new StringBuilder();

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif

    }

}
