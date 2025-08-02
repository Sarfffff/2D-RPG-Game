using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType { 
    big,
    mid,
    small
}



public class Enemy_Slime : Enemy
{

    [SerializeField] private SlimeType Slimetype;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private int slimeToCreate; //分裂的数量
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBatteleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBatteleState(this, stateMachine, "Move", this);  //
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Dead", this);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        
    }
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

        if(Slimetype == SlimeType.small)
            return;
        CreateSlime(slimeToCreate,slimePrefab);
    }
    public void CreateSlime(int _AmoutOfSlime,GameObject _slimeprefab)
    {
        for(int i = 0; i < _AmoutOfSlime; i++)
        {
            GameObject newSlime = Instantiate(_slimeprefab,transform.position,Quaternion.identity);

            newSlime.GetComponent<Enemy_Slime>().SetupSlimeVelocity(facingDir);
        }

        
    }
    private void SetupSlimeVelocity(int _facingDir)
    {
        if (_facingDir != facingDir)
            Filp();
        float xVelocity = Random.Range(minCreationVelocity.x,maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y,maxCreationVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -facingDir, yVelocity);

        Invoke("CancelKnockBack", 1.5f);

    }
    private void CancelKnockBack()=>isKnocked = false;
}
