using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour   //所有的技能都继承Skill方法，减少复用的代码，包括冷却时间，以及是否能够使用技能                                 
{                                   //定义了技能的基本属性和行为，为所有具体技能类提供了一个通用的框架。
                                    //通过继承 Skill 类，各个技能类可以复用 Skill 类的基本属性和方法，如冷却时间管理和查找最近敌人的功能。
                                    //这样可以减少代码重复，提高代码的可维护性和可扩展性。当需要添加新的技能时，只需继承 Skill 类并实现自己的 UseSkill() 方法即可。
    [SerializeField] public float cooldown;
    public float cooldownTimer;//冷却时间


    protected Player player;
    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        CheckUnlock();
    }
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void CheckUnlock()
    {

    }

    public virtual bool CanUseSkill()//提供一个虚函数，子类可以重写
    {
        if (cooldownTimer < 0)   //冷却时间<0
        {
            UseSkill();
            cooldownTimer = cooldown;  //冷却时间重置
            return true;//可以使用技能
        }
        player.playerFx.CreatePopUpText("冷却中");
        return false;
    }
    public virtual void UseSkill()//使用技能，提供接口
    {

    }
    protected virtual Transform FindCloseEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        float closeDistance = Mathf.Infinity;// 初始化一个无穷大的距离，用于比较找到最近的敌人  
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);// 计算当前位置与碰撞体位置之间的距离

                if (distanceToEnemy < closeDistance)// 如果计算出的距离小于当前记录的最小距离 
                {
                    closeDistance = distanceToEnemy;// 更新最小距离
                    closestEnemy = hit.transform;   // 记录最近的敌人
                }
            }
        }
        return closestEnemy;
    }
}
