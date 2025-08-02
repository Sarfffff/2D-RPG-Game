using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState  //��ʼ������׷�ϵ��ˣ�ƽa
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
        if(triggerCalled)  //�ڹ������������һ֡����EnemyAnimationTrigger�ű����������еķ�����triggerCalled��ΪTrue��������ִ����ϣ�����battlestate��׷Ѱplayer
            stateMachine.ChangeState(enemy.battleState);//�ٶ�Ϊ0����ʼ����
    }
}
