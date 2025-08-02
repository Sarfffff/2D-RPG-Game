using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunState : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonStunState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);//眩晕时调用该函数，红色闪烁
        stateTimer = enemy.stunDuration;
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);//enemy的面对方向*眩晕方向的速度
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelColorChange", 0);//眩晕结束，取消红色闪烁
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);  //眩晕时间结束转为静止
    }
}
