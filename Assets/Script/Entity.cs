using System.Collections;
using UnityEngine;


public class Entity : MonoBehaviour
{


    [Header("碰撞检测射线")]
    public Transform attackCheck; //
    public float attackCheckRadius;  //攻击半径

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("角色翻转")]
    private bool facingRight = true; //默认朝向右侧
    public int knockbackDir {  get; private set; }
    public int facingDir { get; private set; } = 1;

    [Header("击退")]
    [SerializeField] protected Vector2 knockbackPower; //击退方向的位移
    protected bool isKnocked; //是否被击退
    [SerializeField] protected float knockBackDuration; //击退时间

    public System.Action onFilpped;
  
    #region 组件
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

    #region 冰冻效果
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

    #region 设置速度
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
    #region 碰撞
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    // 在 Entity 类中添加调试代码
    //public virtual Vector2 DetectCloestGround(Vector2 playerPosition)
    //{
    //    Debug.DrawRay(playerPosition, Vector2.down * 10f, Color.red, 2f); // 可视化射线
    //    RaycastHit2D hit = Physics2D.Raycast(playerPosition, Vector2.down, 10f, whatIsGround);
    //    if (hit.collider != null)
    //    {
    //        Debug.Log("Ground hit at: " + hit.point);
    //        return hit.point;
    //    }
    //    return Vector2.zero;
    //}
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    public virtual void OnDrawGizmos()//检测
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance *facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position,attackCheckRadius);//画一个球体
    }
    #endregion
    #region 翻转
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
    public void SetupKnocwbackPower(Vector2 _knockbackPower) => knockbackPower = _knockbackPower; //巨大伤害才会knockback
    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;
        //if(knockbackPower.x>0||knockbackPower.y>0)
            rb.velocity  = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);  //被击中后，朝反方向产生击退的位移效果

        yield return new WaitForSeconds(knockBackDuration);  //等待击退时间后，击退效果结束，变为false
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
