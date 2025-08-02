using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState  //开始攻击，追上敌人，平a
{
    private Enemy_Skeleton enemy;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimerAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
        if(triggerCalled)  //在攻击动画的最后一帧加上EnemyAnimationTrigger脚本，调用其中的方法将triggerCalled变为True，代表动画执行完毕，进入battlestate，追寻player
            stateMachine.ChangeState(enemy.battleState);//速度为0，开始攻击
    }
}
