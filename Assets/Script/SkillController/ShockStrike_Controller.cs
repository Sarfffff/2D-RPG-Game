using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private Character_Stats targetStats;  //����Ŀ��
    [SerializeField] private float speed;
    private Animator anim;

    private bool triggered;

    private int damage;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    public void SetUp(int _damage, Character_Stats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }
    void Update()
    {
        if (!targetStats)
            return;
        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);  //�����Ŀ��
        transform.right = transform.position - targetStats.transform.position;
        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)  //Ŀ���λ�õ������λ�þ���С��.1f
        {
            anim.transform.localPosition = new Vector3(0, .5f); //�������
            anim.transform.localRotation = Quaternion.identity;//�����ö���ľֲ���ת����Ϊ����ת״̬��

            transform.localRotation = Quaternion.identity;//���õ�ǰ�������ת��
            transform.localScale = new Vector3(3, 3);//����ζ�����彫��X��Y�����ϱ��Ŵ�ԭ����3������Z���򱣳ֲ��䡣

            Invoke("DamageAmdSelfDestory", .2f);
            triggered = true;
            anim.SetTrigger("Hit");  //���ù����Ķ���
        }
    }
    private void DamageAmdSelfDestory()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);  //����˺�
        Destroy(gameObject, .4f);  //����������� 

    }
}
