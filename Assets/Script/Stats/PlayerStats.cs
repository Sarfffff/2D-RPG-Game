using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Character_Stats
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
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
        player.Die();
        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
    protected override void DecreaseHealthy(int _damage)
    {
        base.DecreaseHealthy(_damage);
        if(_damage > GetMaxHealthValue() * .3f)
        {
            player.SetupKnocwbackPower(new Vector2(10, 7));//伤害超过最大的30%，会产生击退
            player.playerFx.ScreenShake(player.playerFx.shakeHighDamage); //受到高额伤害，会产生震动
            int randomSound = Random.Range(31, 32);
            AudioManager.instance.PlaySFX(randomSound, null);
            
        }
        ItemData_Equipment currentArmor = Inventory.Instance.GetEquipment(EquipmentType.armor);
        if (currentArmor != null)
        {
            currentArmor.Effect(player.transform);
        }
    }
    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }
    public void CloneDoDamage(Character_Stats _targetStats, float _multipie)//幻象造成的伤害为最初伤害  * 倍率
    {
        if (canAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();//总伤害
        if(_multipie > 0)
            totalDamage = Mathf.RoundToInt(totalDamage  *_multipie);

        if (CanCrit())  //如果这刀暴击，进入暴击伤害计算 
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);    //选择武器造成伤害
        DoMagicalDamage(_targetStats);           //选择魔法造成伤害
    }
}
