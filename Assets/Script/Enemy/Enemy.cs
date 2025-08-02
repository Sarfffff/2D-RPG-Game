using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFx))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    public LayerMask whatIsPlayer;
    [Header("运动")]
    public float moveSpeed;
    public float idleTime;
    [HideInInspector]public float defaultMoveSpeed;

    [Header("攻击")]
    public float agroDistance = 2f;
    public float attackDistance;
    public float attackCooldown;
    public float battleTime;

    [Header("眩晕")]
    public  Vector2 stunDirection;
    public float stunDuration;
    protected bool canBeStunned;
    [SerializeField] protected  GameObject counterImage;


    [HideInInspector] public float lastTimerAttacked;
    public EnemyStateMachine stateMachine { get; private set; }
    public EntityFx fx { get; private set; }

    //第二种死亡
    public string lastAnimBoolName {  get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed =moveSpeed;  //记录最开始的速度

    }
    protected override void Start()
    {
        base.Start();
        fx = GetComponent<EntityFx>();
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }
    #region 冻结敌人
    public virtual void FreezeTimer(bool _timerFrozen)
    {
        if(_timerFrozen) //true 则将enemy的速度设置为0，并且关闭动画
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else   //则将enemy的速度设置为初始，并且开启动画
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }
    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));
    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTimer(true);
        yield return new WaitForSeconds(_seconds);  //等待0.7f后为false ，冻结时间结束
        FreezeTimer(false);
    }
    #endregion
    public virtual void AnimationFinishTrigger() =>stateMachine.currentState.AnimationFinishTrigger();
    public virtual void AnimationSpecialFinishTrigger() {

        
    }
    public virtual RaycastHit2D IsplayerDetected() => Physics2D.Raycast(wallCheck.position,Vector2.right*facingDir,50,whatIsPlayer);
    //从地面检测的位置发射射线检测什么是玩家
    public virtual void OpenCounterAttackWindow()   //可以眩晕，将组件激活
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()//不可以眩晕，将组件失活
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)  
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;  //黄色的线
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x * attackDistance * facingDir,transform.position.y));
        //当前的位置为起点画线，朝向面对的方向 * 攻击距离 ，得到攻击距离
    }

    //第二种死亡
    public virtual void AssginLastAnimName(string _animBoolname)
    {
        lastAnimBoolName = _animBoolname;
    }
    public override void SlowEntityBy(float _slowPercentage, float slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage); 
        anim.speed = anim.speed * (1- _slowPercentage);
        Invoke("ReturnDefaultSpeed", slowDuration);
       
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }
}
