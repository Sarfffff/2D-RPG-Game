using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;//���ⲿһ��ѡ��
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // canCreateClone = true;
        
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);//��player�Ĺ����ж�����ΪԲ��
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Arror_Controller>() != null)
            {
                hit.GetComponent<Arror_Controller>().Filp();
                SuccessfulCounterAttack();
            }
            if (hit.GetComponent<Enemy>() != null)  //�����ҹ����ж������Ƿ���enemy���
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())  //���enemy�Ƿ���Ա�ѣ��
                {
                    SuccessfulCounterAttack();
                    player.playerFx.ScreenShake(player.playerFx.defaultImpactShack);
                    player.skill.parry.UseSkill();  //�����ָ�

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.parry.MakeMirageOnParry(hit.transform);

                    }

                }
            }
        }
        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void SuccessfulCounterAttack()
    {
        stateTimer = 10;
        player.anim.SetBool("SuccessfulCounterAttack", true);
    }
}
