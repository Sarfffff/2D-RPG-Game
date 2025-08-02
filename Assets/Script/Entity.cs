using System.Collections;
using UnityEngine;


public class Entity : MonoBehaviour
{


    [Header("��ײ�������")]
    public Transform attackCheck; //
    public float attackCheckRadius;  //�����뾶

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("��ɫ��ת")]
    private bool facingRight = true; //Ĭ�ϳ����Ҳ�
    public int knockbackDir {  get; private set; }
    public int facingDir { get; private set; } = 1;

    [Header("����")]
    [SerializeField] protected Vector2 knockbackPower; //���˷����λ��
    protected bool isKnocked; //�Ƿ񱻻���
    [SerializeField] protected float knockBackDuration; //����ʱ��

    public System.Action onFilpped;
  
    #region ���
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public SpriteRenderer sr { get; private set; }
    public Character_Stats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        stats = GetComponent<Character_Stats>();
        cd = GetComponent<CapsuleCollider2D>();
    }
    protected virtual void Update()
    {
        
    }

    #region ����Ч��
    public virtual void SlowEntityBy(float _slowPercentage,float slowDuration)
    {

    }
    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }
    #endregion
    public virtual void DamageImapct()
    {
        StartCoroutine("HitKnockBack");
    }

    #region �����ٶ�
    public void SetZeroVelocity() {
        if(isKnocked)
            return;
        rb.velocity = new Vector2(0, 0);
    } 

    public void SetVelocity(float _Xvelocity, float _Yvelocity)
    {
        if(isKnocked)
            return; 

        rb.velocity = new Vector2(_Xvelocity, _Yvelocity);
        FlipController(_Xvelocity);
    }
    #endregion
    #region ��ײ
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    // �� Entity ������ӵ��Դ���
    //public virtual Vector2 DetectCloestGround(Vector2 playerPosition)
    //{
    //    Debug.DrawRay(playerPosition, Vector2.down * 10f, Color.red, 2f); // ���ӻ�����
    //    RaycastHit2D hit = Physics2D.Raycast(playerPosition, Vector2.down, 10f, whatIsGround);
    //    if (hit.collider != null)
    //    {
    //        Debug.Log("Ground hit at: " + hit.point);
    //        return hit.point;
    //    }
    //    return Vector2.zero;
    //}
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    public virtual void OnDrawGizmos()//���
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance *facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position,attackCheckRadius);//��һ������
    }
    #endregion
    #region ��ת
    public virtual  void SetupKnockBackDir(Transform _damageDirection)
    {
        if(_damageDirection.position.x > transform.position.x)
        {
            knockbackDir = -1;
        }
        else if(_damageDirection.position.x < transform.position.x)
        {
            knockbackDir = 1;
        }
    }

    public void Filp()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        onFilpped?.Invoke();
    }
    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Filp();
        else if (_x < 0 && facingRight)
            Filp();
    }
    #endregion
    public void SetupKnocwbackPower(Vector2 _knockbackPower) => knockbackPower = _knockbackPower; //�޴��˺��Ż�knockback
    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;
        //if(knockbackPower.x>0||knockbackPower.y>0)
            rb.velocity  = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);  //�����к󣬳�������������˵�λ��Ч��

        yield return new WaitForSeconds(knockBackDuration);  //�ȴ�����ʱ��󣬻���Ч����������Ϊfalse
        isKnocked = false;

        SetuoZeroKnockbackPower();
    }
    protected virtual void SetuoZeroKnockbackPower()
    {

    }
    public virtual void Die()
    {

    }
}
