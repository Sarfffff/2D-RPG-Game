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
        if (!player.IsWallDetected())//在进入Update就检测墙体，如果不是则进入air状态
        {   //避免连续按下空格后，进入airstate
            stateMachine.ChangeState(player.airState);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            stateMachine.ChangeState(player.wallJump);
            return;  //在使用蹬墙跳后，直接退回return，不执行下列的代码
        }
        //首先检测是否有方向输入，如果有并且于人物的朝向方向不相同，则从墙上落下
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
