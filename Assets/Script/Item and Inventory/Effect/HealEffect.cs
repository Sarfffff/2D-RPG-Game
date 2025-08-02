using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/Heal effect")]
public class HealEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;//��������ưٷֱ�
    public override void ExecuteEffect(Transform _respawnPosition)
    {
 
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);//������ = player�����Ѫ�� * ������Ƶİٷֱ� �����ȡ��
        playerStats.IncreaseHealthBy(healAmount);
    
    
    }
}
