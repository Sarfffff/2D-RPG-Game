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
        stateTimer = 1f;// ����ǽ��״̬�ĳ���ʱ��
        player.SetVelocity(5 * -player.facingDir,player.jumpForce);  //����ǽ�壬��������Ծ
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
            if (player.IsWallDetected())  //��ǽ�������󣬼���Ƿ�Ϊǽ�壬�������������ڻ�ǽ
            {
                stateMachine.ChangeState(player.wallSlide);
            }
            else
            {
                stateMachine.ChangeState(player.airState);
            }
        }
        if (player.IsGroundDetected())  //��ÿ�ν���������е����⣬��״̬��Ϊidle
            stateMachine.ChangeState(player.idleState); 
    }
}
