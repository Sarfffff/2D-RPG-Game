using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
  private Player player =>GetComponentInParent<Player>();    //得到父类的player组件
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
    private  void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(1, null);
        //定义一个碰撞数组，检测攻击半径内的所有碰撞体，将其存入数组
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        //遍历数组，如果碰撞器中存在Enemy，则对其造成伤害
        foreach(var  hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();//获取敌人的组件对敌人造成伤害
                if(_target != null) 
                    player.stats.DoDamage(_target);

                //Inventory.Instance.GetEquipment(EquipmentType.weapon).Effect(_target.transform);这个没有装备也会产生效果报错

                // 获取武器数据并触发效果，如果没有则是白刀
                ItemData_Equipment weaponData = Inventory.Instance.GetEquipment(EquipmentType.weapon);
                if(weaponData != null) 
                    weaponData.Effect(_target.transform);
            
            }

        }
    }
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreatSword();
    }
}
