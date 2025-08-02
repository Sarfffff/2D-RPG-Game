using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyExplodeController : MonoBehaviour
{
    private Animator anim;
    private Character_Stats myStats;
    private float growSpeed = 15;  //�����ںڶ������������Ĵ�С�Լ��ٶ� + ��֤�뾶
    private float maxSize = 6;
    private float explosionRadius;

    private bool canGrow = true;

    private void Update()
    {
        if (canGrow)
        {  //���
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            explosionRadius = transform.localScale.x / 2; // ��̬������ը�뾶
        }

        if (maxSize - transform.localScale.x < .01f)
        {
            canGrow = false;
            anim.SetTrigger("Explode");
        }
    }


    //��ʼ�� 
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
        //�����ײ�뾶�еĴ�����ײ��������gameobject
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hit in colliders)  //������ײ������
        { //���������Character_Stats��������壬��ɻ����Լ��˺�
            if (hit.GetComponent<Character_Stats>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                myStats.DoDamage(hit.GetComponent<Character_Stats>());
            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);
}