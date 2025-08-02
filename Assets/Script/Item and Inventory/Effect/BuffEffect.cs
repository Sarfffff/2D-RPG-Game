using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class BuffEffect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;  //ö�����ͣ������������ȵ�
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;
    public override void ExecuteEffect(Transform _enemyPosition)
    {

        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetType(buffType));//����Э���Լ����ڵĲ������������ӵ���ֵ��������ʱ�䣬�Լ�����Ч�������ͣ�ö�����ͣ�
    }
    
    
}
