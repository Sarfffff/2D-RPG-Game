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
        //����һ����ײ���飬��⹥���뾶�ڵ�������ײ�壬�����������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        //�������飬�����ײ���д���Enemy�����������˺�
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();//��ȡ��ҵ�������룬Dodamge���������˺�
                enemy.stats.DoDamage(_target);
            }

        }
    }
    private void SpecialAttackTrigger()
    {
        enemy.AnimationSpecialFinishTrigger();
    }
    private void OpenCounterWindow() =>enemy.OpenCounterAttackWindow();  //�ڶ����е��øú�������ʾͼƬ
    private void CloseCounterWindow() =>enemy.CloseCounterAttackWindow();
}
