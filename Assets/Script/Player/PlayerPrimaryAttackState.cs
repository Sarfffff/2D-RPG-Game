using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastTimeAttacked;//检查是否为连击或者单机  在一段时间内如果连续左键即为连击，否则为单机
    private float comboWindow = 2;//0,1,2三次攻击
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // player.SetAttacking(true);
        player.SetZeroVelocity();
        if (comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow)
            comboCounter = 0;
        player.anim.SetInteger("ComboCounter", comboCounter);
        stateTimer = .1f;
        float attackDir;
        #region Choose attack direction
        if (xInput != 0)
            attackDir = xInput;

        attackDir = player.facingDir;
        #endregion
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);//每段攻击设置不一样的速度，x和y轴进行攻击位移
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .15f);
        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)//不能跑打
            player.SetZeroVelocity();//在进入攻击状态后，设置stateTimer计时器，在计时器时间结束后，人物速度设置为0,攻击时速度为0
        if (triggerCalled)//用来在一次攻击后，恢复为静止状态，（点击一次，攻击一次），动画结束后，进入静止状态
            stateMachine.ChangeState(player.idleState);
    }
}
