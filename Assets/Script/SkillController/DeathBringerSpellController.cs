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
        //检测碰撞半径中的带有碰撞器的所有gameobject
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize,whatIsplayer);

        foreach (var hit in colliders)  //遍历碰撞器数组
        { //如果检测带有Character_Stats组件的物体，造成击退以及伤害
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
