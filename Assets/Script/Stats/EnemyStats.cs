using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Character_Stats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stat soulDropAmout;//掉落数

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]//限制percentageModifier
    [SerializeField] private float percentageModifier = .4f;


    protected override void Start()
    {
        soulDropAmout .SetDefaultValue(100);//默认100
        ApplyLevelModifers();
        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModifers()
    {

        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        //Modify(critPower);
        //Modify(critChance);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(fireDamage);
        Modify(lightningDamage);
        
        Modify(soulDropAmout);//随等级几何增加
    }

    private void Modify(Stat _stat)
    {
        for (int i = 0; i < level; i++)
        {
            float modifer = _stat.GetValue() * percentageModifier;//modifer = 基础值 * 随机的（0，1）
            _stat.AddModifers(Mathf.RoundToInt(modifer));//将modifer取整添加到AddModifers中，每次循环都改变一次
        }


    }

    public override void DoDamage(Character_Stats _targetStats)
    {
        base.DoDamage(_targetStats);
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();
        PlayerManager.instance.currency += soulDropAmout.GetValue();
        myDropSystem.GenerateDrop();
        Destroy(gameObject,5f);
    }
}
