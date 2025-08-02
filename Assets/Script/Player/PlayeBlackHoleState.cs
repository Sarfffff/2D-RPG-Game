
using UnityEngine;

public class PlayeBlackHoleState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravity;
    public PlayeBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        skillUsed  = false;
        stateTimer = flyTime;
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale =  defaultGravity;
        player.playerFx.MakeTransprent(false);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }

        if(stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);
            if (!skillUsed)
            {
               if( player.skill.blackhole.CanUseSkill())
                    skillUsed = true;
            }
        }
        if (player.skill.blackhole.SkillCompleted())
            stateMachine.ChangeState(player.airState);

    }
}
