using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    [SerializeField] private float ColorloosingSpeed;
    private Animator anim;

    private float attackMultipier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    private Transform closestEnemy;

    private float cloneTimer;


    private bool canDuplicateClone;  //镜像产生clone
    private int facingDir = 1;
    private float chanceToDuplicate;  //外部调正镜像产生镜像攻击的概率
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //player = PlayerManager.instance.player;
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;    
        if(cloneTimer < 0)
        {
            sr.color = new Color(1,1,1,sr.color.a -(Time.deltaTime* ColorloosingSpeed));
            if (sr.color.a < 0)
            {
                Destroy(gameObject);
            }
        
        }
    }
    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset,Transform _closestEnemy,bool _canDuplicateClone,float _chanceToDuplicate,Player _player,float _attackMultipier) //设置克隆对象的位置
    {
        if(_canAttack) 
            anim.SetInteger("AttackNumber",Random.Range(1,4));
        attackMultipier = _attackMultipier;
        transform.position = _newTransform.position + _offset;
        cloneTimer  = _cloneDuration;
        player = _player;
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;
        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }
    private void AttackTrigger()
    {
        //定义一个碰撞数组，检测攻击半径内的所有碰撞体，将其存入数组
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        //遍历数组，如果碰撞器中存在Enemy，则对其造成伤害
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //player.stats.DoDamage(hit.GetComponent<Character_Stats>());
                hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>(); //获取敌人的stats

                playerStats.CloneDoDamage(enemyStats, attackMultipier);//对敌人造成伤害

                if (player.skill.clone.canApplyOnHitEffect)//如果可以应用击中效果,幻象的攻击也会造成武器的效果
                {
                    ItemData_Equipment weaponData = Inventory.Instance.GetEquipment(EquipmentType.weapon);

                    if (weaponData != null)//应用武器特殊效果
                        weaponData.Effect(hit.transform);


                    if (canDuplicateClone)
                    {
                        if (Random.Range(1, 100) < chanceToDuplicate)
                        {
                            SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(1.5f * facingDir, 0));
                        }
                    }
                }

            }
        }
    }
    private void FaceClosestTarget()
    {

        if (closestEnemy != null)  
        {
            if (transform.position.x > closestEnemy.position.x) //敌人的右侧
            {
                // 直接设置旋转角度，让对象面向敌人
                facingDir = -1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else  
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
