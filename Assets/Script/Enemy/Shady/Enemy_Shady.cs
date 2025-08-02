using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Enemy_Shady : Enemy
{
    [Header ("暗影法师特殊信息")]
    public float battleSpeed;
    [SerializeField] private GameObject explosivePrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;

    #region 状态机
    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    public ShadyStunnedState stunnedState { get; private set; }
    public ShadyDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);

        deadState = new ShadyDeadState(this, stateMachine, "Dead", this);

        battleState = new ShadyBattleState(this, stateMachine, "MoveFast", this);  //
        stunnedState = new ShadyStunnedState(this, stateMachine, "Stunned", this);
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
    }
    public override void AnimationSpecialFinishTrigger()
    {
        GameObject newExplosive = Instantiate(explosivePrefab, transform.position, Quaternion.identity);

        newExplosive.GetComponent<ShadyExplodeController>().SetupExplosive(stats, growSpeed, maxSize, attackDistance);


        cd.enabled = false;
        rb.gravityScale = 0;
    }

    public void SelfDestroy() => Destroy(gameObject);

                        
}
