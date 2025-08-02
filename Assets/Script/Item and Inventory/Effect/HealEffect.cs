using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/Heal effect")]
public class HealEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;//随机的治疗百分比
    public override void ExecuteEffect(Transform _respawnPosition)
    {
 
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);//治疗量 = player的最大血量 * 随机治疗的百分比 ，最后取整
        playerStats.IncreaseHealthBy(healAmount);
    
    
    }
}
