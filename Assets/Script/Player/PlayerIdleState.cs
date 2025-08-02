using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    // Start is called before the first frame update
    public override void Update()
    {
        base.Update();
       if(xInput!= 0 && !player.isBusy)//0&& xInput != player.facingDir )
            stateMachine.ChangeState(player.MoveState);
    }
}
