using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyGroundState : EnemyState
{
    protected Transform player;
    protected Enemy_Shady enemy;
    public ShadyGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsplayerDetected() || Vector2.Distance(player.transform.position, enemy.transform.position) < enemy.agroDistance)
            stateMachine.ChangeState(enemy.battleState);
    }
}
