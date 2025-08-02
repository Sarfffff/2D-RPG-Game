using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    protected Transform player; 
    protected Enemy_DeathBringer enemy;
    protected int moveDir;
    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {

        this.enemy = enemy;
    }

    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform; //获取player的transform组件,前提是进入射线检测到player
        //if (player.GetComponent<PlayerStats>().isDead) //玩家死亡，不会寻找玩家
        //    stateMachine.ChangeState(enemy.moveState);
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

        if (player.position.x > enemy.transform.position.x)//如果人物在骷髅的右边，骷髅朝向右侧
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)//如果人物在骷髅的左边，骷髅朝向左侧
            moveDir = -1;

        if (enemy.IsplayerDetected() && enemy.IsplayerDetected().distance < enemy.attackDistance - .1f)
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
