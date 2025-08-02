using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class BuffEffect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;  //枚举类型，包括攻击力等等
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;
    public override void ExecuteEffect(Transform _enemyPosition)
    {

        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetType(buffType));//传入协程以及对于的参数，包括增加的数值，持续的时间，以及增益效果的类型（枚举类型）
    }
    
    
}
