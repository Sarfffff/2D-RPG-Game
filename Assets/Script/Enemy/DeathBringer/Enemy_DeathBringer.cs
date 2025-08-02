using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
    public bool bossFightBegun;

    [Header("施法信息")]//Spell cast details
    [SerializeField] private GameObject spellPrefab;
    public int amountOfSpells; //spell技能是否次数
    public float spellCooldown;

    public float lastTimeCast;
    [SerializeField] private float spellStateCooldown;
    [SerializeField] private Vector2 spellOffset;



    [Header("传送")]
    [SerializeField] private BoxCollider2D arena;   //传送的范围区域
    [SerializeField] private Vector2 surroundingCheckSize;    //周围环境的检查范围
    public float chanceToteleport;
    public float defaultChanceToTeleport = 25;

    #region 状态机
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }
    public DeathBringerSpellCastState spellCastState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);  //
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        deadState = new DeathBringerDeadState(this, stateMachine, "Dead", this);
        spellCastState = new DeathBringerSpellCastState(this, stateMachine, "SpellCast", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3,arena.bounds.max.x-3);  //在区域的范围类随机生成坐标位置
        float y = Random.Range(arena.bounds.min.y + 3,arena.bounds.max.y-3);

        //将随机的位置赋值给当前的位置，进行传送
        transform.position = new Vector2(x,y);  

        //调正y轴的位置，确保传送后正确站立在地面上
        transform.position = new Vector2(transform.position.x,transform.position.y - GrounBelowCheck().distance + (cd.size.y)/2);
        if(!GrounBelowCheck()||somethingIsAround())  //如果周围纯在玩家或者是障碍物，或者是没用检查到地面，重新寻找位置
        {
            FindPosition();
        }
    }
    private RaycastHit2D GrounBelowCheck() => Physics2D.Raycast(transform.position,Vector2.down,100,whatIsGround);  //进行传送后的地面检查
    private bool somethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);//检查是否周围是否有玩家和障碍物

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y - GrounBelowCheck().distance));//从当前的位置 到 地面的垂直线
        Gizmos.DrawWireCube(transform.position,surroundingCheckSize);
    }
    public bool canTeleport()
    {
        if (Random.Range(0, 100) < chanceToteleport)
        {
            chanceToteleport = defaultChanceToTeleport;
            return true;
        }
        return false;
    }
    public void CastSpell()
    {

        Player player = PlayerManager.instance.player;
        float xOffset = 0;
        if (player.rb.velocity.x != 0)  //如果玩家速度不为0，则生成一个偏移量，预测玩家的移动。如果为0，则在生成预制体时，在玩家的头顶生成
            xOffset = player.facingDir * spellOffset.x;

        Vector3 spellPostion = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + spellOffset.y);
            //施法技能的位置 ,如果玩家的朝向为右，则向右偏移，

        GameObject newSpell = Instantiate(spellPrefab, spellPostion, Quaternion.identity);

        newSpell.GetComponent<DeathBringerSpellController>().SetupSpell(stats);

    }
    public bool CanDoSpellCast()  //检查上一次的技能冷却
    {
        if (Time.time >= lastTimeCast + spellStateCooldown)
        {
            return true;
        }
        else
            return false;
    }
}
