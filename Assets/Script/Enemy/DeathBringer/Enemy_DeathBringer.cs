using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
    public bool bossFightBegun;

    [Header("ʩ����Ϣ")]//Spell cast details
    [SerializeField] private GameObject spellPrefab;
    public int amountOfSpells; //spell�����Ƿ����
    public float spellCooldown;

    public float lastTimeCast;
    [SerializeField] private float spellStateCooldown;
    [SerializeField] private Vector2 spellOffset;



    [Header("����")]
    [SerializeField] private BoxCollider2D arena;   //���͵ķ�Χ����
    [SerializeField] private Vector2 surroundingCheckSize;    //��Χ�����ļ�鷶Χ
    public float chanceToteleport;
    public float defaultChanceToTeleport = 25;

    #region ״̬��
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
        float x = Random.Range(arena.bounds.min.x + 3,arena.bounds.max.x-3);  //������ķ�Χ�������������λ��
        float y = Random.Range(arena.bounds.min.y + 3,arena.bounds.max.y-3);

        //�������λ�ø�ֵ����ǰ��λ�ã����д���
        transform.position = new Vector2(x,y);  

        //����y���λ�ã�ȷ�����ͺ���ȷվ���ڵ�����
        transform.position = new Vector2(transform.position.x,transform.position.y - GrounBelowCheck().distance + (cd.size.y)/2);
        if(!GrounBelowCheck()||somethingIsAround())  //�����Χ������һ������ϰ��������û�ü�鵽���棬����Ѱ��λ��
        {
            FindPosition();
        }
    }
    private RaycastHit2D GrounBelowCheck() => Physics2D.Raycast(transform.position,Vector2.down,100,whatIsGround);  //���д��ͺ�ĵ�����
    private bool somethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);//����Ƿ���Χ�Ƿ�����Һ��ϰ���

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y - GrounBelowCheck().distance));//�ӵ�ǰ��λ�� �� ����Ĵ�ֱ��
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
        if (player.rb.velocity.x != 0)  //�������ٶȲ�Ϊ0��������һ��ƫ������Ԥ����ҵ��ƶ������Ϊ0����������Ԥ����ʱ������ҵ�ͷ������
            xOffset = player.facingDir * spellOffset.x;

        Vector3 spellPostion = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + spellOffset.y);
            //ʩ�����ܵ�λ�� ,�����ҵĳ���Ϊ�ң�������ƫ�ƣ�

        GameObject newSpell = Instantiate(spellPrefab, spellPostion, Quaternion.identity);

        newSpell.GetComponent<DeathBringerSpellController>().SetupSpell(stats);

    }
    public bool CanDoSpellCast()  //�����һ�εļ�����ȴ
    {
        if (Time.time >= lastTimeCast + spellStateCooldown)
        {
            return true;
        }
        else
            return false;
    }
}
