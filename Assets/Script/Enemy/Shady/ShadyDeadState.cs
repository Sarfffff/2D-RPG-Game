using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeadState : EnemyState
{
    protected Enemy_Shady enemy;
    public ShadyDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Shady enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }


    public override void Enter()
    {
        base.Enter();

    } 

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            enemy.SelfDestroy();
    }
}
   