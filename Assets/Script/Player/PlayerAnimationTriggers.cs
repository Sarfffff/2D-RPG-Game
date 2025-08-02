using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
  private Player player =>GetComponentInParent<Player>();    //�õ������player���
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
    private  void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(1, null);
        //����һ����ײ���飬��⹥���뾶�ڵ�������ײ�壬�����������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        //�������飬�����ײ���д���Enemy�����������˺�
        foreach(var  hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();//��ȡ���˵�����Ե�������˺�
                if(_target != null) 
                    player.stats.DoDamage(_target);

                //Inventory.Instance.GetEquipment(EquipmentType.weapon).Effect(_target.transform);���û��װ��Ҳ�����Ч������

                // ��ȡ�������ݲ�����Ч�������û�����ǰ׵�
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
