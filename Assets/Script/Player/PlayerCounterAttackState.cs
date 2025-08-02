using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;//给外部一个选项
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);//在player的攻击判定区域为圆形
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Arror_Controller>() != null)
            {
                hit.GetComponent<Arror_Controller>().Filp();
                SuccessfulCounterAttack();
            }
            if (hit.GetComponent<Enemy>() != null)  //检测玩家攻击判顶区域是否有enemy组件
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())  //检测enemy是否可以被眩晕
                {
                    SuccessfulCounterAttack();
                    player.playerFx.ScreenShake(player.playerFx.defaultImpactShack);
                    player.skill.parry.UseSkill();  //弹反恢复

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
