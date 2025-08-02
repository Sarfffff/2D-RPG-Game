using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour   //���еļ��ܶ��̳�Skill���������ٸ��õĴ��룬������ȴʱ�䣬�Լ��Ƿ��ܹ�ʹ�ü���                                 
{                                   //�����˼��ܵĻ������Ժ���Ϊ��Ϊ���о��弼�����ṩ��һ��ͨ�õĿ�ܡ�
                                    //ͨ���̳� Skill �࣬������������Ը��� Skill ��Ļ������Ժͷ���������ȴʱ�����Ͳ���������˵Ĺ��ܡ�
                                    //�������Լ��ٴ����ظ�����ߴ���Ŀ�ά���ԺͿ���չ�ԡ�����Ҫ����µļ���ʱ��ֻ��̳� Skill �ಢʵ���Լ��� UseSkill() �������ɡ�
    [SerializeField] public float cooldown;
    public float cooldownTimer;//��ȴʱ��


    protected Player player;
    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        CheckUnlock();
    }
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void CheckUnlock()
    {

    }

    public virtual bool CanUseSkill()//�ṩһ���麯�������������д
    {
        if (cooldownTimer < 0)   //��ȴʱ��<0
        {
            UseSkill();
            cooldownTimer = cooldown;  //��ȴʱ������
            return true;//����ʹ�ü���
        }
        player.playerFx.CreatePopUpText("��ȴ��");
        return false;
    }
    public virtual void UseSkill()//ʹ�ü��ܣ��ṩ�ӿ�
    {

    }
    protected virtual Transform FindCloseEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        float closeDistance = Mathf.Infinity;// ��ʼ��һ�������ľ��룬���ڱȽ��ҵ�����ĵ���  
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);// ���㵱ǰλ������ײ��λ��֮��ľ���

                if (distanceToEnemy < closeDistance)// ���������ľ���С�ڵ�ǰ��¼����С���� 
                {
                    closeDistance = distanceToEnemy;// ������С����
                    closestEnemy = hit.transform;   // ��¼����ĵ���
                }
            }
        }
        return closestEnemy;
    }
}
