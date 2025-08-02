using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttackState : EnemyState
{
    protected Enemy_Slime enemy;
    public SlimeAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    // Start is called before the first frame update
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
        if (triggerCalled)  //�ڹ������������һ֡����EnemyAnimationTrigger�ű����������еķ�����triggerCalled��ΪTrue��������ִ����ϣ�����battlestate��׷Ѱplayer
            stateMachine.ChangeState(enemy.battleState);//�ٶ�Ϊ0����ʼ����
    }
}
