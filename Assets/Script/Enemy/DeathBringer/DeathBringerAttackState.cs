using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
    protected Enemy_DeathBringer enemy;
    protected Player player;
    public DeathBringerAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.chanceToteleport += 5;
        player = PlayerManager.instance.player;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimerAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
        if (triggerCalled)  //在攻击动画的最后一帧加上EnemyAnimationTrigger脚本，调用其中的方法将triggerCalled变为True，代表动画执行完毕，进入battlestate，追寻player
        {
            if (enemy.canTeleport())
            {
                stateMachine.ChangeState(enemy.teleportState);

            }
            else
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }

    }
}

