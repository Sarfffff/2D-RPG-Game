using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState

{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }
    
    #region ��д����Ĺ��캯��
    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(14, null);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(14);
    }

    public override void Update()
    {
        base.Update();
      
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (xInput==0)
            stateMachine.ChangeState(player.idleState);
        
    }
    #endregion 
}
