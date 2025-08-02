using System.Collections;
using UnityEngine;

//2024.12.11
public class ArcherBattleState : EnemyState
{
    private Transform player;
    private Enemy_Archer enemy;
    private int moveDir;


    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();

        //player = GameObject.Find("Player").transform;
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);

    }


    public override void Update()
    {
        base.Update();

        if (enemy.IsplayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsplayerDetected().distance < enemy.safeDistance)//小于安全距离
            {
                if (CanJump())
                    stateMachine.ChangeState(enemy.ArcherJumpState);//跳走


            }


            if (enemy.IsplayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)//超过距离或者时间到了
                stateMachine.ChangeState(enemy.idleState);
        }

        BattleStateFlipController();

    }

    private void BattleStateFlipController()
    {
        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
            enemy.Filp();
        else if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
            enemy.Filp();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimerAttacked + enemy.attackCooldown)
        {
         
            enemy.lastTimerAttacked = Time.time;
            return true;
        }
        else
            return false;
    }


    private bool CanJump()
    {
        if (enemy.GroundBenhundCheck() == false || enemy.WallBehind() == true)
            return false;

        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }


        return false;
    }
}
