using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    weapon,
    armor,
    Amulet,//»¤Éí·û
    Flask  //Ò©Æ·
}

[CreateAssetMenu(fileName = "New Item data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    [Header("ÌØÐÔ")]
    public float ItemCooldown;
    public ItemEffect[] itemEffect;



    [Header("ÊôÐÔ")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;
    [Header("¹¥»÷")]
    public int damage;
    public int critchance;
    public int critPower;
    [Header("·ÀÓù")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;
    [Header("Ä§·¨")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;
    [Header("Craft requirments")]
    public List<InventoryItem> craftingMaterials;

    private int minDescriptionLength;
    public void Effect(Transform _enemyPosition)
    {
        foreach(var item in itemEffect)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifers(strength);
        playerStats.agility.AddModifers(agility);
        playerStats.intelligence.AddModifers(intelligence);
        playerStats.vitality.AddModifers(vitality);


        playerStats.damage.AddModifers(damage);
        playerStats.critChance.AddModifers(critchance);
        playerStats.critPower.AddModifers(critPower);


        playerStats.maxHealth.AddModifers(health);
        playerStats.armor.AddModifers(armor);
        playerStats.evasion.AddModifers(evasion);
        playerStats.magicResistance.AddModifers(magicResistance);


        playerStats.iceDamage.AddModifers(iceDamage);
        playerStats.fireDamage.AddModifers(fireDamage);
        playerStats.lightningDamage.AddModifers(lightningDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifers(strength);
        playerStats.agility.RemoveModifers(agility);
        playerStats.intelligence.RemoveModifers(intelligence);
        playerStats.vitality.RemoveModifers(vitality);


        playerStats.damage.RemoveModifers(damage);
        playerStats.critChance.RemoveModifers(critchance);
        playerStats.critPower.RemoveModifers(critPower);


        playerStats.maxHealth.RemoveModifers(health);
        playerStats.armor.RemoveModifers(armor);
        playerStats.evasion.RemoveModifers(evasion);
        playerStats.magicResistance.RemoveModifers(magicResistance);


        playerStats.iceDamage.RemoveModifers(iceDamage);
        playerStats.fireDamage.RemoveModifers(fireDamage);
        playerStats.lightningDamage.RemoveModifers(lightningDamage);
    }
    public virtual string GetDescription()
    {
        sb.Length = 0;
        minDescriptionLength = 0;
        AddItemDescription(strength, "Á¦Á¿");
        AddItemDescription(agility, "Ãô½Ý");
        AddItemDescription(intelligence, "ÖÇÁ¦");
        AddItemDescription(vitality, "ÌåÁ¦");

        AddItemDescription(damage, "¹¥»÷Á¦");
        AddItemDescription(critchance, "±©»÷ÂÊ");
        AddItemDescription(critPower, "±©»÷ÉËº¦");

        AddItemDescription(health, "ÉúÃüÖµ");
        AddItemDescription(armor, "»¤¼×");
        AddItemDescription(evasion, "ÉÁ±Ü¼¸ÂÊ");
        AddItemDescription(magicResistance, "Ä§·¨¿¹ÐÔ");

        AddItemDescription(fireDamage, "Ñ×ÓüÉËº¦");
        AddItemDescription(iceDamage, "Ëª»ÃÉËº¦");
        AddItemDescription(lightningDamage, "À×ÕÝÉËº¦");

        for(int i = 0; i < itemEffect.Length; i++)
        {
            if (itemEffect[i].effectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Unique: "+itemEffect[i].effectDescription);
                minDescriptionLength++;
            }    
        }

        if(minDescriptionLength < 5)
        {
            for (int i = 0; i < 5 - minDescriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }


        return sb.ToString();
    }
    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();
            if (_value > 0)
                sb.Append("+"+_value+" "+_name);

            minDescriptionLength++;
        }
    }
}
