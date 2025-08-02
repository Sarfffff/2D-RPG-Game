using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    protected Enemy_Slime enemy;
    public SlimeStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
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
        enemy.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < .1f && enemy.IsGroundDetected())
        {
            enemy.anim.SetTrigger("StunFold");
            enemy.stats.MakeInvincible(true);
            enemy.fx.Invoke("CancelColorChange", 0);//眩晕结束，取消红色闪烁
        }
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);  //眩晕时间结束转为静止
    }
}
