using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyExplodeController : MonoBehaviour
{
    private Animator anim;
    private Character_Stats myStats;
    private float growSpeed = 15;  //类似于黑洞技能设置最大的大小以及速度 + 保证半径
    private float maxSize = 6;
    private float explosionRadius;

    private bool canGrow = true;

    private void Update()
    {
        if (canGrow)
        {  //变大
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            explosionRadius = transform.localScale.x / 2; // 动态调整爆炸半径
        }

        if (maxSize - transform.localScale.x < .01f)
        {
            canGrow = false;
            anim.SetTrigger("Explode");
        }
    }


    //初始化 
    public void SetupExplosive(Character_Stats _myStats, float _growSpeed, float _maxSize, float _radius)
    {
        anim = GetComponent<Animator>();

        myStats = _myStats;
        growSpeed = _growSpeed;
        maxSize = _maxSize;
        explosionRadius = _radius;
    }


    private void AnimationExplodeEvent()
    {
        //检测碰撞半径中的带有碰撞器的所有gameobject
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hit in colliders)  //遍历碰撞器数组
        { //如果检测带有Character_Stats组件的物体，造成击退以及伤害
            if (hit.GetComponent<Character_Stats>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                myStats.DoDamage(hit.GetComponent<Character_Stats>());
            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);
}