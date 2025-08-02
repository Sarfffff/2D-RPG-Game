using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
   private Enemy enemy =>GetComponentInParent<Enemy>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        //定义一个碰撞数组，检测攻击半径内的所有碰撞体，将其存入数组
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        //遍历数组，如果碰撞器中存在Enemy，则对其造成伤害
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();//获取玩家的组件传入，Dodamge对玩家造成伤害
                enemy.stats.DoDamage(_target);
            }

        }
    }
    private void SpecialAttackTrigger()
    {
        enemy.AnimationSpecialFinishTrigger();
    }
    private void OpenCounterWindow() =>enemy.OpenCounterAttackWindow();  //在动画中调用该函数，显示图片
    private void CloseCounterWindow() =>enemy.CloseCounterAttackWindow();
}
