using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDeadState : EnemyState
{
    protected Enemy_Archer enemy;
    public ArcherDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Archer enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    // Start is called before the first frame update
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        //第二种死亡
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;
        stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();
        //1.两种死亡方式
        //第一种
        // enemy.SetZeroVelocity();

        //第二种
        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 10);
    }
}
