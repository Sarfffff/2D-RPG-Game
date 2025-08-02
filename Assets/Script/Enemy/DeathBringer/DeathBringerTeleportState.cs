using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTeleportState : EnemyState
{
    protected Enemy_DeathBringer enemy;
    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.stats.MakeInvincible(true);
        
    }
    public override void Exit() 
    { 
        base.Exit();
        enemy.stats.MakeInvincible(false);
    }
    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            if (enemy.CanDoSpellCast())
            {
                stateMachine.ChangeState(enemy.spellCastState);
            }
            else
                stateMachine.ChangeState(enemy.battleState);
        }
    }

}
