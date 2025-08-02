using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyBattleState : EnemyState
{
    private Enemy_Shady enemy;
    private Transform player;  //��ȡplayer
    private int moveDir;
    private float defaultSpeed;
    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Shady enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform; //��ȡplayer��transform���,ǰ���ǽ������߼�⵽player
        defaultSpeed = enemy.moveSpeed;
        enemy.moveSpeed = enemy.battleSpeed;
        if (player.GetComponent<PlayerStats>().isDead) //�������������Ѱ�����
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed = defaultSpeed;
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsplayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsplayerDetected().distance < enemy.attackDistance)   //������˵ļ�⵽player�ľ���С�ڹ�������,����player�ڹ�����Χ�ڣ�����
            {
                enemy.stats.KillEntity();   
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7f)//ս��ʱ������������������˼�ľ������ > 7f���л�״̬Ϊ��ֹ
                stateMachine.ChangeState(enemy.idleState);

        }

        if (player.position.x > enemy.transform.position.x)//������������õ��ұߣ����ó����Ҳ�
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)//������������õ���ߣ����ó������
            moveDir = -1;

        if (Vector2.Distance(player.transform.position, enemy.transform.position) > 1)
            enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
        else
            enemy.SetZeroVelocity();

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
