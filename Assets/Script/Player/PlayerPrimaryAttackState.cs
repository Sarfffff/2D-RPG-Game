using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastTimeAttacked;//����Ƿ�Ϊ�������ߵ���  ��һ��ʱ����������������Ϊ����������Ϊ����
    private float comboWindow = 2;//0,1,2���ι���
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
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);//ÿ�ι������ò�һ�����ٶȣ�x��y����й���λ��
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

        if (stateTimer < 0)//�����ܴ�
            player.SetZeroVelocity();//�ڽ��빥��״̬������stateTimer��ʱ�����ڼ�ʱ��ʱ������������ٶ�����Ϊ0,����ʱ�ٶ�Ϊ0
        if (triggerCalled)//������һ�ι����󣬻ָ�Ϊ��ֹ״̬�������һ�Σ�����һ�Σ������������󣬽��뾲ֹ״̬
            stateMachine.ChangeState(player.idleState);
    }
}
