using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private Character_Stats targetStats;  //朝向目标
    [SerializeField] private float speed;
    private Animator anim;

    private bool triggered;

    private int damage;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    public void SetUp(int _damage, Character_Stats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }
    void Update()
    {
        if (!targetStats)
            return;
        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);  //朝向的目标
        transform.right = transform.position - targetStats.transform.position;
        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)  //目标的位置到自身的位置距离小于.1f
        {
            anim.transform.localPosition = new Vector3(0, .5f); //从天而降
            anim.transform.localRotation = Quaternion.identity;//它将该对象的局部旋转重置为无旋转状态。

            transform.localRotation = Quaternion.identity;//重置当前物体的旋转。
            transform.localScale = new Vector3(3, 3);//这意味着物体将在X和Y方向上被放大到原来的3倍，而Z方向保持不变。

            Invoke("DamageAmdSelfDestory", .2f);
            triggered = true;
            anim.SetTrigger("Hit");  //调用攻击的动画
        }
    }
    private void DamageAmdSelfDestory()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);  //造成伤害
        Destroy(gameObject, .4f);  //最后销毁闪电 

    }
}
