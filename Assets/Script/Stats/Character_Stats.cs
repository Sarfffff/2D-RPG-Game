using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

// ���幥������ö��
public enum AttackType
{
    Fire,
    Ice,
    Lightning
}

public enum StatType
{
    strength,
    agility,
    intelegence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightingDamage
}


public class Character_Stats : MonoBehaviour
{
    private EntityFx fx;
    public Vector2 offest = new Vector2(1f, 1f);
    [Header("��ɫ����")]
    public Stat strength;  //����   1����������1���˺���1�㱩���˺�
    public Stat agility;//����    �������
    public Stat intelligence;//�ǻ� ����ħ����ħ���˺�
    public Stat vitality;//������  1�����������5��Ѫ��

    [Header("���� ")]//����
    public Stat maxHealth;
    public Stat armor;//����
    public Stat evasion;//����
    public Stat magicResistance;//ħ��


    [Header("����")]//����
    public Stat damage;
    public Stat critChance;//������
    public Stat critPower;//�����˺�

    [Header("ħ��")]
    public Stat iceDamage;
    public Stat fireDamage;
    public Stat lightningDamage;//�׵��˺�

    public bool isIgnited;//ȼ��  ���һ��ʱ����˺�
    public bool isChilled;//����  ����20%�Ļ���
    public bool isShocked;//ѣ��  ����20%��������
    //ʩ�Ӹ���Ч��

    [SerializeField] private float ailmentsDuration = 4;//״̬�쳣ʱ��
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float ignitedDamageCooldown = .3f;
    private float ignitedDamageTimer;

    [SerializeField] private GameObject shockStrikePrefab;//�����
    private int shockDamage;
    private int igniteDamage;


    public System.Action onHealthChanged;//ʹ��ɫ��Stat�����UI��ĺ���
    public bool isDead { get; private set; }
    public bool IsInvincible { get; private set; }

    public int currentHealth;
    private bool isVulnerable;//����״̬
    
  
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);//����150%
        currentHealth = GetMaxHealthValue();//Ѫ�� = ����Ѫ�� + װ�� + ������(�츳)
        fx = GetComponent<EntityFx>();

    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime; //ȼ�ճ�������ʱ�����
        shockedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;


        ignitedDamageTimer -= Time.deltaTime;//ȼ������˺���ʩ�ӳ�������ʱ�����

        if (ignitedTimer < 0)
        {
            isIgnited = false;
   
        }
        if (chilledTimer < 0)
        {
            isChilled = false;
       
        }
        if (shockedTimer < 0)
        {
            isShocked = false;
         
        }

        if (isIgnited)
            ApplyIgniteDamage();

    }

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableCorutine(_duration));//����״̬��Э��

    private IEnumerator VulnerableCorutine(float _duration)//����״̬��Э��
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }


    //�÷�������Ҫ����������һ����Ϊ StatModCorotien ��Э�̣�
    //���� _modifier��_duration �� _statToModeify �������������ݸ�Э�̣��Ӷ���ʼ��ָ�����Խ�����ʱ�޸ĵĲ������̡�
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModeify)
    {  //����Ϊ�����ӵ�ֵ��������ʱ�䣬�޸ĵ����ԣ�
        StartCoroutine(StatModCorotien(_modifier, _duration, _statToModeify));
    }

    private IEnumerator StatModCorotien(int _modifier, float _duration, Stat _statToModeify)
    {

        _statToModeify.AddModifers(_modifier);
        yield return new WaitForSeconds(_duration);//�ȴ�����ʱ������������ʼ��״̬
        _statToModeify.RemoveModifers(_modifier);
    }
    public virtual void DoDamage(Character_Stats _targetStats)
    {

        bool criticalStrike = false;
        if (canAvoidAttack(_targetStats))
            return;

        if(_targetStats.IsInvincible)
            return;
        _targetStats.GetComponent<Entity>().SetupKnockBackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();//���˺�
        if (CanCrit())  //����⵶���������뱩���˺����� 
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            criticalStrike = true;       
        }
 
        fx.CreateHitFx(_targetStats.transform, criticalStrike);
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);    //ѡ����������˺�

         DoMagicalDamage(_targetStats);           //ѡ��ħ������˺�

    }

    protected virtual void Die()
    {
        isDead = true;
    }
    public void KillEntity() {
        if (!isDead)
            Die();
    }

    public void MakeInvincible(bool _invincible)
    {
        IsInvincible = _invincible;
    }
    
    public virtual void TakeDamage(int _damage)
    {
        if (IsInvincible)
            return;
        DecreaseHealthy(_damage);

        fx.StartCoroutine("FlashFX");
        GetComponent<Entity>().DamageImapct();
        
        if (currentHealth < 0 && !isDead)
            Die();
    }


    public virtual void IncreaseHealthBy(int _amount)//����������������Ѫ��ʹ��ȥ������Ѫ
    {
        currentHealth += _amount;  //��ǰ��Ѫ�� = ��ǰѪ��+������
        if (currentHealth > GetMaxHealthValue())  //�����ǰѪ�� �������Ѫ��  ��������Ѫ��
            currentHealth = GetMaxHealthValue();
        if (onHealthChanged != null)
            onHealthChanged();
    }
    protected virtual void DecreaseHealthy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;

        if(_damage > 0)
        {
            if(CanCrit())
                fx.CreatePopUpText("<color=yellow>" + _damage.ToString() + "</color>");
            else
                fx.CreatePopUpText(_damage.ToString());

        }
            
        if (onHealthChanged != null)
            onHealthChanged();
    }


    #region ħ���˺��͸���Ч��
    private void ApplyIgniteDamage()  //ʩ�ӻ���ȼ���˺�Ч��
    {
        if (ignitedDamageTimer < 0 )
        {

            DecreaseHealthy(igniteDamage);//����ȼ������˺�������
            if (currentHealth < 0 && !isDead)
                Die();

            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }


    public virtual void DoMagicalDamage(Character_Stats _targetStats)//���ħ���˺�
    {
        //��ȡ�����˺�ֵ������
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();


        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();//��ħ���˺� = ��+��+�׵�+�����츳
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage); //��ħ���˺� = ħ���˺� - ħ��
        _targetStats.TakeDamage(totalMagicDamage);  //�������˺�

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)//��������˺�����0���˳�
            return;


        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }
    //�����ָ���Ч����ɵ��˺����бȽϣ����ָ���Ч��ǿʹ���ĸ����������������������ʩ��һ���쳣״̬
    private void AttemptToApplyAilments(Character_Stats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;//ȼ��Ч��
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyChill && !canApplyShock && !canApplyIgnite)  //
        {
            int result = Random.Range(1, 4);
            if (result == 1 && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;

            }
            if (result == 2 && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (result == 3 && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

        }
        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * .1f));


        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }


    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)//Ӧ��״̬Ч��
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;
        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;//��ȼ2s
            fx.IgniteFxfor(ailmentsDuration);
            //ShowInjurySprite(AttackType.Fire); // ��ʾ��ȼ����
        }
        if (_chill && canApplyChill)
        {
            chilledTimer = ailmentsDuration;
            isChilled = _chill;
            float slowPercentage = .2f;//���ٰٷֱ�    m
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxfor(ailmentsDuration);
           // ShowInjurySprite(AttackType.Ice); // ��ʾ���ᾫ��
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
               // ShowInjurySprite(AttackType.Lightning); // ��ʾ�������
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;

                HitNearestTargetWithShockStrike();
            }
        }
    }



    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;
        shockedTimer = ailmentsDuration;
        isShocked = _shock;
        fx.ShockFxfor(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closeDistance = Mathf.Infinity;// ��ʼ��һ�������ľ��룬���ڱȽ��ҵ�����ĵ���  
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);// ���㵱ǰλ������ײ��λ��֮��ľ���

                if (distanceToEnemy < closeDistance)// ���������ľ���С�ڵ�ǰ��¼����С���� 
                {
                    closeDistance = distanceToEnemy;// ������С����
                    closestEnemy = hit.transform;   // ��¼����ĵ���
                }
            }
            if (closestEnemy == null)  //
                closestEnemy = transform;

        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().SetUp(shockDamage, closestEnemy.GetComponent<Character_Stats>());
        }
    }//�׵�һ    ��

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion


    #region Stat calculations

    private int CheckTargetResistance(Character_Stats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        //�ܵ�ħ���˺� = �ܵ�ħ���˺� - ��ħ�� + ����*3��//һ������ ���3��ħ��
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }


    public int CheckTargetArmor(Character_Stats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)//���䶳
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();
        totalDamage -= _targetStats.armor.GetValue(); //�����˺�ֵ��totalDamage���м�ȥĿ�껤��ֵ��_targetStats.armor.GetValue()��
                                                      //_targetStats��һ������Ŀ�����ͳ����Ϣ�Ķ���armor�����е�һ�����ԣ�����Ŀ��Ļ��ף���GetValue()��������������ȡ�������ֵ�ľ�����ֵ

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);//Mathf.Clamp����ȷ��totalDamage��ֵ�������0��Ҳ���ᳬ��int.MaxValue��
        return totalDamage;
    }
    public virtual void OnEvasion()
    {

    }
    public bool canAvoidAttack(Character_Stats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();//������ = Ŀ�꿪ʼ������ֵ + Ŀ�꿪ʼ�����ݶȵõ�����ֵ

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }//����

    public bool CanCrit()  //����
    {
        int totalCritcakChance = critChance.GetValue() + agility.GetValue();//������ = �����ı�����+����ֵ
        if (Random.Range(0, 100) <= totalCritcakChance)
        {
            return true;
        }
        return false;
    }

    public int CalculateCriticalDamage(int _Damage)//_Damage������ͨ�������˺�
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;//�����˺� = ��������150% + ����
        float critDamage = _Damage * totalCritPower;//����ͨ�˺�_Damage����totalCritPower���õ������˺�
        return Mathf.RoundToInt(critDamage);  //���������ʾ����
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;

    }//ͳ������ֵ����


    #endregion

    public Stat GetType(StatType _StatType)
    {
        if (_StatType == StatType.strength) return strength;
        else if (_StatType == StatType.agility) return agility;
        else if (_StatType == StatType.intelegence) return intelligence;
        else if (_StatType == StatType.vitality) return vitality;
        else if (_StatType == StatType.damage) return damage;
        else if (_StatType == StatType.critChance) return critChance;
        else if (_StatType == StatType.critPower) return critPower;
        else if (_StatType == StatType.health) return maxHealth;
        else if (_StatType == StatType.armor) return armor;
        else if (_StatType == StatType.evasion) return evasion;
        else if (_StatType == StatType.magicRes) return magicResistance;
        else if (_StatType == StatType.fireDamage) return fireDamage;
        else if (_StatType == StatType.iceDamage) return iceDamage;
        else if (_StatType == StatType.lightingDamage) return lightningDamage;
        return null;
    }


}