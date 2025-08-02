using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : Entity
{


    [Header("????")]
    public Vector2[] attackMovement;  //????¦Ë??
    public float counterAttackDuration = .2f;
    public bool isBusy {  get; private set; }   

    [Header("???")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;
    private float DefaultMoveSpeed;
    private float DefaultJumpSpeed;


    [Header("???")]
    public float dashSpeed;
    public float dashDuration;
    private float DefaultDashSpeed;
    public float dashDir {  get; private set; }

    public SkillManager skill {  get; private set; }
    public GameObject sword { get; private set; }
    public PlayerFx playerFx { get; private set; }


    #region ????
    public PlayerStateMachine stateMachine {  get; private set; }
    public PlayerState idleState {  get; private set; }
    public PlayerState MoveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }  
    public  PlayerWallSlideState wallSlide { get; private set; }
    public  PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimaryAttackState primaryattack {  get; private set; }
    public  PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSowrd { get; private set; }
    public PlayerCatchSwordState catchSword{get; private set;}
    public PlayeBlackHoleState blackHole { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion



    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();//???????????????????
        idleState = new PlayerIdleState(this, stateMachine,"Idle");
        MoveState = new PlayerMoveState(this,stateMachine,"Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this,stateMachine,"WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryattack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this,stateMachine,"CounterAttack");
        aimSowrd = new PlayerAimSwordState(this,stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHole = new PlayeBlackHoleState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");

    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState); //???????idle??
        skill = SkillManager.instance;
        playerFx = GetComponent<PlayerFx>();
        DefaultMoveSpeed = moveSpeed;
        DefaultJumpSpeed = jumpForce;
        DefaultDashSpeed = dashSpeed;
        
    }
    protected override void Update()
    {
        base.Update();
        if (Time.timeScale ==0)
            return;

        stateMachine.currentState.Update(); //?????§Þ???????? ???§Ý???????
        CheckForDashInput ();
        if (Input.GetKeyDown(KeyCode.F)&&skill.crystal.crystalUnlocked)
            skill.crystal.CanUseSkill();
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.Instance.UsedFlask();
        
    }
    public void AssignNewSword(GameObject _newSword)
    {
        sword  = _newSword;
    }
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }
    public void ExitBlakcHoleAbility()
    {
        stateMachine.ChangeState(airState);
    }
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }
    public void AnimationTrigger() =>stateMachine.currentState.AnimationFinishTrigger(); 
    //?????????????????????§Ü????????????AnimationFinishTrigger(),??triggerCalled???true


    private void CheckForDashInput()
    {
        if(IsWallDetected()&&!IsGroundDetected() )
            return;
        if(skill.dash.dashUnlocked == false)  //¦Ä???????????
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift)&& SkillManager.instance.dash.CanUseSkill()) {
            dashDir = Input.GetAxisRaw("Horizontal"); //????????x????????
            if(dashDir==0)  //??§Ù???????dash?????????
                dashDir = facingDir;

                stateMachine.ChangeState(dashState);
        }
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
    protected override void SetuoZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }
    //public override Vector2 DetectCloestGround(Vector2 playerPosition)
    //{
    //    return base.DetectCloestGround(playerPosition);
    //}
    #region ????§¹?? ???????
    public override void SlowEntityBy(float _slowPercentage, float slowDuration)
    {
        moveSpeed = moveSpeed * (1 -_slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);
        Invoke("ReturnDefaultSpeed", slowDuration);

    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = DefaultMoveSpeed;
        jumpForce = DefaultJumpSpeed;
        dashSpeed = DefaultDashSpeed;

    }
    #endregion
}
