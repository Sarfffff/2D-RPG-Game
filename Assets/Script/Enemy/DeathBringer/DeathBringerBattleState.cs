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
        player = PlayerManager.instance.player.transform; //��ȡplayer��transform���,ǰ���ǽ������߼�⵽player
        //if (player.GetComponent<PlayerStats>().isDead) //�������������Ѱ�����
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

            if (enemy.IsplayerDetected().distance < enemy.attackDistance)   //������˵ļ�⵽player�ľ���С�ڹ�������,����player�ڹ�����Χ�ڣ�����
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);

                }

            }
        }

        if (player.position.x > enemy.transform.position.x)//������������õ��ұߣ����ó����Ҳ�
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)//������������õ���ߣ����ó������
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
