using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBatteleState : EnemyState
{
    protected Transform player;

    protected Enemy_Slime enemy;
    private int moveDir;
    private Animator anim;

    public SlimeBatteleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform; //获取player的transform组件,前提是进入射线检测到player
        if (player.GetComponent<PlayerStats>().isDead) //玩家死亡，不会寻找玩家
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsplayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsplayerDetected().distance < enemy.attackDistance)   //如果敌人的检测到player的距离小于攻击距离,就是player在攻击范围内，脸上
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);

                }

            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7f)//战斗时间结束或者是玩家与敌人间的距离大于 > 7f，切换状态为静止
                stateMachine.ChangeState(enemy.idleState);
            stateTimer = 0;              
        }

        if (player.position.x > enemy.transform.position.x)//如果人物在Slime的右边，骷髅朝向右侧
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)//如果人物在Slime的左边，骷髅朝向左侧
            moveDir = -1;

        if (enemy.IsplayerDetected() && enemy.IsplayerDetected().distance < enemy.attackDistance - .5f)
        {
            return;
        }
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);

    }
    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimerAttacked + enemy.attackCooldown)
        {
            return true;
        }
        return false;
    }
}
