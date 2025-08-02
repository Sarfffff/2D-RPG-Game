using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    protected bool triggerCalled;
    protected string animBoolName;
    protected Rigidbody2D rb;

    protected float stateTimer;
    public EnemyState (Enemy _enemyBase,EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        
    }
    public virtual void Enter()
    {
        //ÿ�ν���triggerCalled ���ᱻ��ʼ��Ϊ false������ζ��ÿ�ν�����״̬ʱ��Ĭ�϶�����δ������ɡ�
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName,true);
    }
    public virtual void Exit() {

        enemyBase.anim.SetBool(animBoolName,false);
        //�ڶ�������
        enemyBase.AssginLastAnimName(animBoolName);
    
    }
    public virtual void AnimationFinishTrigger()
    {
        //������ִ����ϣ�����״̬ת��
        triggerCalled = true;
    }
}
