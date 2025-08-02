using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(0,0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackHoleUnlocked)
        {
            if (player.skill.blackhole.cooldownTimer > 0)
            {
                player.playerFx.CreatePopUpText("冷却中");
                return;
            }
            stateMachine.ChangeState(player.blackHole);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword()&&player.skill.sword.swordUnlocked)
            stateMachine.ChangeState(player.aimSowrd);

        if (Input.GetKeyDown(KeyCode.Q)&&player.skill.parry.parryUnlocked &&SkillManager.instance.parry.CanUseSkill())
            stateMachine.ChangeState(player.counterAttack);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryattack);

        if (player.IsGroundDetected() == false)
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space)&& player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
        
    }
    private bool HasNoSword()
    {
        if (!player.sword) //检测玩家是否有剑，如果有，return true,如果没有，调用Sword_Skill_Controller脚本中的返回剑的函数，得到剑
        {
            return true;
        }
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
