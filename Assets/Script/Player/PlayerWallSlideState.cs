using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!player.IsWallDetected())//�ڽ���Update�ͼ��ǽ�壬������������air״̬
        {   //�����������¿ո�󣬽���airstate
            stateMachine.ChangeState(player.airState);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            stateMachine.ChangeState(player.wallJump);
            return;  //��ʹ�õ�ǽ����ֱ���˻�return����ִ�����еĴ���
        }
        //���ȼ���Ƿ��з������룬����в���������ĳ�������ͬ�����ǽ������
        if (player.facingDir != xInput && xInput != 0) {
            stateMachine.ChangeState(player.idleState);
        }
        if(yInput < 0)
            rb.velocity = new Vector2(0,rb.velocity.y);
        else
            rb.velocity = new Vector2(0,rb.velocity.y *.7f);
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
    
}
