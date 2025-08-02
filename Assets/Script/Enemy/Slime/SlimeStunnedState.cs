using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    protected Enemy_Slime enemy;
    public SlimeStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);//ѣ��ʱ���øú�������ɫ��˸
        stateTimer = enemy.stunDuration;
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);//enemy����Է���*ѣ�η�����ٶ�
    }

    public override void Exit()
    {
        base.Exit();
        enemy.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < .1f && enemy.IsGroundDetected())
        {
            enemy.anim.SetTrigger("StunFold");
            enemy.stats.MakeInvincible(true);
            enemy.fx.Invoke("CancelColorChange", 0);//ѣ�ν�����ȡ����ɫ��˸
        }
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);  //ѣ��ʱ�����תΪ��ֹ
    }
}
