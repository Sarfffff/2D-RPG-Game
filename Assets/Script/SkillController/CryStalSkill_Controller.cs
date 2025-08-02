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
    {//�ӽӴ��ĵ��˵��б����ѡ����˵�λ��Ϊ�����Ŀ��
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
        //����һ����ײ���飬��⹥���뾶�ڵ�������ײ�壬�����������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        //�������飬�����ײ���д���Enemy�����������˺�
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                player.stats.DoMagicalDamage(hit.GetComponent<Character_Stats>());

                ItemData_Equipment equipedAmulet = Inventory.Instance.GetEquipment(EquipmentType.Amulet);//��ȡAmuletװ���������û�գ��򹥻����ˣ�����װ��Ч��
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
