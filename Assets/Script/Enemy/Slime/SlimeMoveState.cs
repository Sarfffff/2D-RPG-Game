using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : SlimeGroundedState
{
    public SlimeMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Filp();
            stateMachine.ChangeState(enemy.idleState);

        }

    }
}
