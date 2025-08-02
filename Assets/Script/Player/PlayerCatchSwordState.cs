
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();  //�ڽ����յ�ʱ�򣬽�����ķ����� �����յķ���
        sword = player.sword.transform;

        player.playerFx.PlayDustFx();
        player.playerFx.ScreenShake(player.playerFx.defaultImpactShack);
        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
            player.Filp();
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
            player.Filp();
        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.velocity.y);//���ս��ĺ�����
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();
        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
