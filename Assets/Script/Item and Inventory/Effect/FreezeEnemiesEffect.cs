using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Freeze enemies", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;//冻结时间

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f) //玩家血量 < 10%才会触发
            return;

        if (!Inventory.Instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemyPosition.position, 2);//检测半径内的所有的碰撞器，并且对所有的敌人造成damage
        foreach (var hit in colliders)
        {

            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);


        }
    }
}
