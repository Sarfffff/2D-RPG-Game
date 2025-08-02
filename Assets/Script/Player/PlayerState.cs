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
    private string animBoolName;  //�������õĶ������֣���Move��Idle

    protected float stateTimer;

    protected bool triggerCalled;

    //���캯��
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
        //ÿ�ν���triggerCalled ���ᱻ��ʼ��Ϊ false������ζ��ÿ�ν�����״̬ʱ��Ĭ�϶�����δ������ɡ�
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
