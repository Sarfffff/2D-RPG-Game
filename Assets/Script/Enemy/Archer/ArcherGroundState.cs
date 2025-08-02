using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024.12.11
public class ArcherGroundState : EnemyState
{

    protected Transform player;
    protected Enemy_Archer enemy;

    public ArcherGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;//p63 3:43¸Ä
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if (enemy.IsplayerDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.agroDistance)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}