using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    protected float xInput;
    protected float yInput;
    protected Rigidbody2D rb;

    protected PlayerStateMachine stateMachine;
    protected Player player;
    private string animBoolName;  //传入设置的动画名字，如Move，Idle

    protected float stateTimer;

    protected bool triggerCalled;

    //构造函数
    public PlayerState (Player _player, PlayerStateMachine _stateMachine, string animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _player;
        this.animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        //每次进入triggerCalled 都会被初始化为 false，这意味着每次进入新状态时，默认动画还未播放完成。
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity",rb.velocity.x);
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
    public virtual void AnimationFinishTrigger()
    {
       
        triggerCalled = true;
    }
    
}
