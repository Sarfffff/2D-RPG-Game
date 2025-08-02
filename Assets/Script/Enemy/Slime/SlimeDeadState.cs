using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeadState : EnemyState
{
    protected Enemy_Slime enemy;
    public SlimeDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        ////�ڶ�������
        //enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        //enemy.anim.speed = 0;
        //enemy.cd.enabled = false;
        //stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();
        //1.����������ʽ
        //��һ��
         enemy.SetZeroVelocity();

        ////�ڶ���
        //if (stateTimer > 0)
        //    rb.velocity = new Vector2(0, 10);
    }
}
