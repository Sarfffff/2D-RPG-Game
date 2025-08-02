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
    [Header("�˶�")]
    public float moveSpeed;
    public float idleTime;
    [HideInInspector]public float defaultMoveSpeed;

    [Header("����")]
    public float agroDistance = 2f;
    public float attackDistance;
    public float attackCooldown;
    public float battleTime;

    [Header("ѣ��")]
    public  Vector2 stunDirection;
    public float stunDuration;
    protected bool canBeStunned;
    [SerializeField] protected  GameObject counterImage;


    [HideInInspector] public float lastTimerAttacked;
    public EnemyStateMachine stateMachine { get; private set; }
    public EntityFx fx { get; private set; }

    //�ڶ�������
    public string lastAnimBoolName {  get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed =moveSpeed;  //��¼�ʼ���ٶ�

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
    #region �������
    public virtual void FreezeTimer(bool _timerFrozen)
    {
        if(_timerFrozen) //true ��enemy���ٶ�����Ϊ0�����ҹرն���
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else   //��enemy���ٶ�����Ϊ��ʼ�����ҿ�������
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }
    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));
    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTimer(true);
        yield return new WaitForSeconds(_seconds);  //�ȴ�0.7f��Ϊfalse ������ʱ�����
        FreezeTimer(false);
    }
    #endregion
    public virtual void AnimationFinishTrigger() =>stateMachine.currentState.AnimationFinishTrigger();
    public virtual void AnimationSpecialFinishTrigger() {

        
    }
    public virtual RaycastHit2D IsplayerDetected() => Physics2D.Raycast(wallCheck.position,Vector2.right*facingDir,50,whatIsPlayer);
    //�ӵ������λ�÷������߼��ʲô�����
    public virtual void OpenCounterAttackWindow()   //����ѣ�Σ����������
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()//������ѣ�Σ������ʧ��
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
        Gizmos.color = Color.yellow;  //��ɫ����
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x * attackDistance * facingDir,transform.position.y));
        //��ǰ��λ��Ϊ��㻭�ߣ�������Եķ��� * �������� ���õ���������
    }

    //�ڶ�������
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
