using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellController : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsplayer;


    private Character_Stats myStats;

    public void SetupSpell(Character_Stats _myStats)
    {
       myStats = _myStats;  
    }
    private void AnimationTrigger()
    {
        //�����ײ�뾶�еĴ�����ײ��������gameobject
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize,whatIsplayer);

        foreach (var hit in colliders)  //������ײ������
        { //���������Character_Stats��������壬��ɻ����Լ��˺�
            if (hit.GetComponent<Character_Stats>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                myStats.DoDamage(hit.GetComponent<PlayerStats>());
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position,boxSize);
    }
    private void SelfDestroy() => Destroy(gameObject);
}
