using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Freeze enemies", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;//����ʱ��

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f) //���Ѫ�� < 10%�Żᴥ��
            return;

        if (!Inventory.Instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemyPosition.position, 2);//���뾶�ڵ����е���ײ�������Ҷ����еĵ������damage
        foreach (var hit in colliders)
        {

            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);


        }
    }
}
