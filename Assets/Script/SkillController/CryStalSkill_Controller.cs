using UnityEngine;

public class CryStalSkill_Controller : MonoBehaviour
{
    private Animator anim =>GetComponent<Animator>();
    private CircleCollider2D cd =>GetComponent<CircleCollider2D>();

    private Player player;

    private float crystalExitTimer;
    private bool canMove;
    private float moveSpeed;
    private bool canExplode;

    private bool canGrow;
    private float growSpeed =5f ;

    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;
    public void ChooseRandomEnemy()
    {//从接触的敌人的列表随机选择敌人的位置为最近的目标
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if(colliders.Length > 0) 
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }
    public void SetupCrystal(float _crystalDuartion,bool _canExplode,bool _canMove,float _moveSpeed,Transform _closestTarget,Player _player)
    {
        player = _player;
        crystalExitTimer = _crystalDuartion;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }

    private void Update()
    {
        crystalExitTimer -= Time.deltaTime;
        if (crystalExitTimer < 0) 
            FinishCrystal();
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale,new Vector2(3,3),growSpeed * Time.deltaTime);
        if (canMove)
        {
            if(closestTarget ==null)
                return;
            transform.position = Vector2.MoveTowards(transform.position,closestTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
                canMove = false;
            }
        }

    }
    private void AnimationExplodeEvent()
    {
        //定义一个碰撞数组，检测攻击半径内的所有碰撞体，将其存入数组
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        //遍历数组，如果碰撞器中存在Enemy，则对其造成伤害
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                player.stats.DoMagicalDamage(hit.GetComponent<Character_Stats>());

                ItemData_Equipment equipedAmulet = Inventory.Instance.GetEquipment(EquipmentType.Amulet);//获取Amulet装备，如果不没空，则攻击敌人，产生装备效果
                if(equipedAmulet != null)
                {
                    equipedAmulet.Effect(hit.transform);
                }

            }

        }
    }
    public void FinishCrystal()
    {
        if (canExplode) {
            canGrow = true;
            anim.SetTrigger("Explode");
        }   
        else
            SelfDestory();
    }

    public void SelfDestory() =>Destroy(gameObject);
}
