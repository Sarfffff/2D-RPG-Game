using System.Collections;
using UnityEngine;

public class ArcherMoveState : ArcherGroundState
{

    public ArcherMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

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

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())//撞墙或者没有路反转
        {
            enemy.Filp();

            stateMachine.ChangeState(enemy.idleState);
        }
    }
}