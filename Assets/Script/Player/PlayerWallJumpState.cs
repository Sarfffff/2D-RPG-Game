using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 1f;// 限制墙跳状态的持续时间
        player.SetVelocity(5 * -player.facingDir,player.jumpForce);  //跳出墙体，反方向跳跃
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            if (player.IsWallDetected())  //在墙跳结束后，检测是否为墙体，如果是则继续处于滑墙
            {
                stateMachine.ChangeState(player.wallSlide);
            }
            else
            {
                stateMachine.ChangeState(player.airState);
            }
        }
        if (player.IsGroundDetected())  //在每次进入地面后进行地面检测，将状态变为idle
            stateMachine.ChangeState(player.idleState); 
    }
}
