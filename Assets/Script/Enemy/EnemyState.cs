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
        //每次进入triggerCalled 都会被初始化为 false，这意味着每次进入新状态时，默认动画还未播放完成。
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName,true);
    }
    public virtual void Exit() {

        enemyBase.anim.SetBool(animBoolName,false);
        //第二中死亡
        enemyBase.AssginLastAnimName(animBoolName);
    
    }
    public virtual void AnimationFinishTrigger()
    {
        //代表动画执行完毕，进行状态转换
        triggerCalled = true;
    }
}
