using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

// 定义攻击类型枚举
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
    [Header("角色属性")]
    public Stat strength;  //力量   1点力量增加1点伤害和1点暴击伤害
    public Stat agility;//敏捷    提高闪避
    public Stat intelligence;//智慧 增加魔抗和魔法伤害
    public Stat vitality;//生命力  1点生命力提高5点血量

    [Header("防御 ")]//防御
    public Stat maxHealth;
    public Stat armor;//护甲
    public Stat evasion;//闪避
    public Stat magicResistance;//魔抗


    [Header("攻击")]//攻击
    public Stat damage;
    public Stat critChance;//暴击率
    public Stat critPower;//暴击伤害

    [Header("魔法")]
    public Stat iceDamage;
    public Stat fireDamage;
    public Stat lightningDamage;//雷电伤害

    public bool isIgnited;//燃烧  造成一段时间的伤害
    public bool isChilled;//冻结  减少20%的护甲
    public bool isShocked;//眩晕  减少20%的命中率
    //施加负面效果

    [SerializeField] private float ailmentsDuration = 4;//状态异常时间
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float ignitedDamageCooldown = .3f;
    private float ignitedDamageTimer;

    [SerializeField] private GameObject shockStrikePrefab;//冲击波
    private int shockDamage;
    private int igniteDamage;


    public System.Action onHealthChanged;//使角色在Stat里调用UI层的函数
    public bool isDead { get; private set; }
    public bool IsInvincible { get; private set; }

    public int currentHealth;
    private bool isVulnerable;//易伤状态
    
  
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);//爆伤150%
        currentHealth = GetMaxHealthValue();//血量 = 基础血量 + 装备 + 生命力(天赋)
        fx = GetComponent<EntityFx>();

    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime; //燃烧持续随桢时间减少
        shockedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;


        ignitedDamageTimer -= Time.deltaTime;//燃烧造成伤害的施加持续随桢时间减少

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

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableCorutine(_duration));//易伤状态的协程

    private IEnumerator VulnerableCorutine(float _duration)//易伤状态的协程
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }


    //该方法的主要作用是启动一个名为 StatModCorotien 的协程，
    //并将 _modifier、_duration 和 _statToModeify 这三个参数传递给协程，从而开始对指定属性进行临时修改的操作流程。
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModeify)
    {  //参数为（增加的值，持续的时间，修改的属性）
        StartCoroutine(StatModCorotien(_modifier, _duration, _statToModeify));
    }

    private IEnumerator StatModCorotien(int _modifier, float _duration, Stat _statToModeify)
    {

        _statToModeify.AddModifers(_modifier);
        yield return new WaitForSeconds(_duration);//等待持续时间结束，返回最开始的状态
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

        int totalDamage = damage.GetValue() + strength.GetValue();//总伤害
        if (CanCrit())  //如果这刀暴击，进入暴击伤害计算 
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            criticalStrike = true;       
        }
 
        fx.CreateHitFx(_targetStats.transform, criticalStrike);
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);    //选择武器造成伤害

         DoMagicalDamage(_targetStats);           //选择魔法造成伤害

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


    public virtual void IncreaseHealthBy(int _amount)//传入治疗量，供饮血剑使用去增加饮血
    {
        currentHealth += _amount;  //当前的血量 = 当前血量+治疗量
        if (currentHealth > GetMaxHealthValue())  //如果当前血量 大于最大血量  则等于最大血量
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


    #region 魔法伤害和负面效果
    private void ApplyIgniteDamage()  //施加火焰燃烧伤害效果
    {
        if (ignitedDamageTimer < 0 )
        {

            DecreaseHealthy(igniteDamage);//火焰燃烧造成伤害，真伤
            if (currentHealth < 0 && !isDead)
                Die();

            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }


    public virtual void DoMagicalDamage(Character_Stats _targetStats)//造成魔法伤害
    {
        //获取最终伤害值并传入
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();


        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();//总魔法伤害 = 冰+火+雷电+智力天赋
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage); //总魔法伤害 = 魔法伤害 - 魔抗
        _targetStats.TakeDamage(totalMagicDamage);  //最后造成伤害

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)//如果所有伤害都是0，退出
            return;


        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }
    //将三种负面效果造成的伤害进行比较，那种负面效果强使用哪个，都不满足条件则尝试随机施加一种异常状态
    private void AttemptToApplyAilments(Character_Stats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;//燃烧效果
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


    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)//应用状态效果
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;
        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;//点燃2s
            fx.IgniteFxfor(ailmentsDuration);
            //ShowInjurySprite(AttackType.Fire); // 显示点燃精灵
        }
        if (_chill && canApplyChill)
        {
            chilledTimer = ailmentsDuration;
            isChilled = _chill;
            float slowPercentage = .2f;//减速百分比    m
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxfor(ailmentsDuration);
           // ShowInjurySprite(AttackType.Ice); // 显示冻结精灵
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
               // ShowInjurySprite(AttackType.Lightning); // 显示电击精灵
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
        float closeDistance = Mathf.Infinity;// 初始化一个无穷大的距离，用于比较找到最近的敌人  
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);// 计算当前位置与碰撞体位置之间的距离

                if (distanceToEnemy < closeDistance)// 如果计算出的距离小于当前记录的最小距离 
                {
                    closeDistance = distanceToEnemy;// 更新最小距离
                    closestEnemy = hit.transform;   // 记录最近的敌人
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
    }//雷电一    击

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion


    #region Stat calculations

    private int CheckTargetResistance(Character_Stats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        //总的魔法伤害 = 总的魔法伤害 - （魔抗 + 智力*3）//一点智力 提高3点魔抗
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }


    public int CheckTargetArmor(Character_Stats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)//被冷冻
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();
        totalDamage -= _targetStats.armor.GetValue(); //从总伤害值（totalDamage）中减去目标护甲值（_targetStats.armor.GetValue()）
                                                      //_targetStats是一个包含目标各种统计信息的对象，armor是其中的一个属性，代表目标的护甲，而GetValue()方法则是用来获取这个护甲值的具体数值

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);//Mathf.Clamp方法确保totalDamage的值不会低于0，也不会超过int.MaxValue（
        return totalDamage;
    }
    public virtual void OnEvasion()
    {

    }
    public bool canAvoidAttack(Character_Stats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();//总闪避 = 目标开始的闪避值 + 目标开始的敏捷度得到的数值

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }//闪避

    public bool CanCrit()  //暴击
    {
        int totalCritcakChance = critChance.GetValue() + agility.GetValue();//暴击率 = 基础的暴击率+闪避值
        if (Random.Range(0, 100) <= totalCritcakChance)
        {
            return true;
        }
        return false;
    }

    public int CalculateCriticalDamage(int _Damage)//_Damage代表普通攻击的伤害
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;//暴击伤害 = 基础爆伤150% + 力量
        float critDamage = _Damage * totalCritPower;//将普通伤害_Damage乘以totalCritPower来得到最终伤害
        return Mathf.RoundToInt(critDamage);  //四舍五入表示整数
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;

    }//统计生命值函数


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