using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private float returnSpeed = 12f;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool canRotate =true ;
    private bool isReturning;
    private float FreezeTimeDuration;
    [Header("弹跳")]
    private float bounceSpeed; //弹跳速度
    private bool isBouncing;  //反弹
    private int bounceAmount; //来回弹跳次数
    private List<Transform> enemyTarget;//敌人列表，存储剑的碰撞器接触的敌人
    private int targetIndex;  //敌人列表索引

    [Header("穿刺")]
    [SerializeField] private float pierceAmount;

    [Header("旋转")]
    private float maxTravelDistance;  //最大移动距离
    private float spinDuration;  //旋转持续时间
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCoolDown;

    private float spinDirection;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }
    private void DestoryMe()
    {
        Destroy(gameObject);
    }

    #region 初始化各种类型的剑
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player,float _FreezeTimeDuraion,float _returnSpeed)
    {
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;//重力
        returnSpeed = _returnSpeed;
        FreezeTimeDuration = _FreezeTimeDuraion;
        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
        Invoke("DestoryMe", 7);
    }
    public void SetupBounce(bool _isBouncing,int _bouncesAmount,float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _bouncesAmount;
        bounceSpeed = _bounceSpeed;
        enemyTarget = new List<Transform>();
    }
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance,float _spinDuration,float _hitCoolDown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCoolDown = _hitCoolDown;
            
    }
    #endregion


    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;//运动学关闭
        transform.parent = null;  //父物体设置为null
        isReturning = true;  //开始回收剑
    }


    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)//回收sword，每帧检测是否为null
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)  //sword的距离与玩家的距离<2,也就是在回收的过程中，
                player.CatchTheSword();//剑离玩家越来越近的过程中，小于2，则销毁物体（不显示sword）

        }
        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();

            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                //发射出去也会也会向前运动
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f*Time.deltaTime);
                if (spinTimer < 0)
                {
                    isReturning = true; //旋转持续时间到期
                    isSpinning = false;

                }

                hitTimer -= Time.deltaTime;//将攻击冷却时间传入
                if (hitTimer < 0)
                {
                    hitTimer = hitCoolDown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1); //以当前对象（可能是角色）的位置为中心，半径为1的所有碰撞体
                    foreach (var hit in colliders)  //每0.35f造成一次伤害
                    {
                        if (hit.GetComponent<Enemy>() != null)
                           SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //Rigidbody2D 组件的所有约束都设为冻结状态，也就是说该刚体在 2D 物理模拟时位置和旋转都不会改变。
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            //它使角色朝向enemyTarget列表中当前targetIndex指定的敌人目标移动。移动的速度由bounceSpeed乘以Time.deltaTime决定，以确保移动是平滑的且与帧率无关。

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            { //这行代码检查角色与当前目标敌人之间的距离是否小于0.1个单位。如果是，表示角色已经接近或达到了该目标。     
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;//指向下一个敌人，索引增加
                bounceAmount--;//反弹次数减少
                if (bounceAmount <= 0)//检查amountOfBounce是否小于或等于0。如果是，表示没有更多的反弹目标或反弹次数已用完
                {
                    isBouncing = false;  //反弹结束
                    isReturning = true;//剑返回
                }
                if (targetIndex >= enemyTarget.Count)//检查targetIndex是否大于或等于enemyTarget列表的长度。如果是，表示已经超出了目标列表的范围。（超过检测范围，剑则不攻击。）
                    targetIndex = 0;//将targetIndex重置为0，以便重新开始从第一个敌人目标进行反弹攻击
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)  //当剑的触发器检测到物体时，进行操作
    {//停止旋转、禁用某个组件、将刚体设置为运动学模式、冻结刚体的所有约束以及将剑的父对象设置为碰撞到的物体。
        if (isReturning)
            return;  //旋转收回

        if (collision.GetComponent<Enemy>() != null)  //触发器检测到敌人，造成伤害并且冻结
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }
        SetupTargetForBounce(collision);
        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        player.stats.DoDamage(enemy.GetComponent<Character_Stats>());
        EnemyStats enemystats = enemy.GetComponent<EnemyStats>();

        if(player.skill.sword.timeStopUnlocked)//技能解锁才能使用
            enemy.FreezeTimeFor(FreezeTimeDuration);

        if(player.skill.sword.volnurableUnlocked)
            enemystats.MakeVulnerableFor(FreezeTimeDuration);



        ItemData_Equipment equipedAmulet = Inventory.Instance.GetEquipment(EquipmentType.Amulet);//获取Amulet装备，如果不没空，则攻击敌人，产生装备效果
        if (equipedAmulet != null)
        {
            equipedAmulet.Effect(enemy.transform);
        }
    }

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)//人物前的碰撞点得到（即检测）到敌人的组件
        {
            if (isBouncing && enemyTarget.Count <= 0)//只有当剑在反弹且没有当前攻击目标时，这个条件才为真
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10); //以当前对象（可能是角色）的位置为中心，半径为10的所有碰撞体
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform); //将当前遍历到的碰撞体的transform属性（表示其在场景中的位置和方向）添加到enemyTarget列表中。这意味着将当前检测到的碰撞体（可能是敌人）标记为攻击目标
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null) {
            pierceAmount--;
            return;
            
        }
        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponentInChildren<ParticleSystem>().Play();

        if(isBouncing && enemyTarget.Count > 0)
            return ;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
