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
            player.SetupKnocwbackPower(new Vector2(10, 7));//�˺���������30%�����������
            player.playerFx.ScreenShake(player.playerFx.shakeHighDamage); //�ܵ��߶��˺����������
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
    public void CloneDoDamage(Character_Stats _targetStats, float _multipie)//������ɵ��˺�Ϊ����˺�  * ����
    {
        if (canAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();//���˺�
        if(_multipie > 0)
            totalDamage = Mathf.RoundToInt(totalDamage  *_multipie);

        if (CanCrit())  //����⵶���������뱩���˺����� 
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);    //ѡ����������˺�
        DoMagicalDamage(_targetStats);           //ѡ��ħ������˺�
    }
}
