using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherJumpState : EnemyState
{
    protected Enemy_Archer enemy;
    public ArcherJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.rb.velocity = new Vector2(enemy.jumpVelocity.x * -enemy.facingDir, enemy.jumpVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.anim.SetFloat("yVelocity", rb.velocity.y);
        if (rb.velocity.y < 0 && enemy.IsGroundDetected())
            stateMachine.ChangeState(enemy.battleState);
    }
}
