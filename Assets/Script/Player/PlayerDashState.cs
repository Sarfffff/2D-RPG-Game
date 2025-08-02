using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
       player.skill.dash.CloneOnDash();
        stateTimer = player.dashDuration;
        player.stats.MakeInvincible(true);

    }

    public override void Exit()
    {
        base.Exit();
        player.skill.dash.CloneOnDashArrival();
        player.SetVelocity(0, rb.velocity.y);
        player.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();
        stateTimer -=Time.deltaTime;
        if (!player.IsGroundDetected() && player.IsWallDetected()) //ÔÚÇ½ÉÏ
            stateMachine.ChangeState(player.wallSlide);
        player.SetVelocity(player.dashSpeed * player.dashDir,0);
        if (stateTimer < 0)//³å´Ì½áÊø
            stateMachine.ChangeState(player.idleState);
        player.playerFx.CreateAfterImage();
    }
}
