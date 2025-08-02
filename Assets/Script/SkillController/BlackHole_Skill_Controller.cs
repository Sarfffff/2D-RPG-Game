using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;//���ߴ�
    private float growSpeed;//����ٶ�
    private float shrinkSpeed;//��С�ٶ�
    private float blackholeTimer;

    private bool canGrow = true;//�Ƿ���Ա��
    private bool canShrink;//��С
    private bool canCreateHotKeys = true; //ר�ſ��ƺ�������û�������ȼ�
    private bool cloneAttackReleased;
    private bool playerCanDisapear = true;

    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;



    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createHotKey = new List<GameObject>();

    public bool playerCanExitState { get; private set; }
    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;
        if (SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDisapear = false;
    }
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;
        if(blackholeTimer <= 0 ) //�ڶ�����ʱ�����
        {
            blackholeTimer = Mathf.Infinity;  //��ʱ������Ϊ���޳�
            if(targets.Count > 0)   //�����������>0���ͷŹ�����û����ʼ�����˳��Ĺ���
                ReleaseCloneAttack();  
            else
                FinishBlackHoleAbility();
        }


        if (Input.GetKeyUp(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            //���ǿ��������С�Ĳ���
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            //����MoveToward�������ǷŴ󵽶��ٴ�С \
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0, 0), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x <= 1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if(targets.Count <=0)
            return;

        DestroyHotKey();
        cloneAttackReleased = true;
        canCreateHotKeys = false;
        if (playerCanDisapear)
        {
            playerCanDisapear = false;
            PlayerManager.instance.player.playerFx.MakeTransprent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased &&amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);

            float xoffSet;
            if (Random.Range(0, 100) > 50)  //x�����ƫ����
                xoffSet = 1.5f;
            else
                xoffSet = -1.5f;

            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xoffSet, 0)); //�����Ŀ�¡������ƫ����

            }
            amountOfAttacks--;
            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleAbility", 1f);
            }
        }
    }

    private void FinishBlackHoleAbility()    //�����˳���������ɺڶ�����
    {
        DestroyHotKey();
        playerCanExitState = true;  //�˳�
        canShrink = true;  //��С
        cloneAttackReleased = false;   
        
    }

    private void DestroyHotKey()
    {
        if (createHotKey.Count <= 0)
            return;
        for (int i = 0; i < createHotKey.Count; i++)
            Destroy(createHotKey[i]);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTimer(true);
            CreateHotKey(collision);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTimer(false);

        }
    }
    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
            return;
        if (!canCreateHotKeys)
            return;
         //����ʵ��
        GameObject newHotkey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        //��ʵ����ӽ��б�
        createHotKey.Add(newHotkey);
        //���KeyCode����HotKey�����Ҵ���ȥһ���ٵ�һ��
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];

        keyCodeList.Remove(choosenKey);

        BlackHotKey_Controller newHotkeyScript = newHotkey.GetComponent<BlackHotKey_Controller>();
        newHotkeyScript.SetupHotKey(choosenKey, collision.transform, this);
        //newHotkey.GetComponent<BlackHole_Skill_Controller>.SetupHotKey();
        // targets.Add(collision.transform);
    }
    public void AddEnemyToList(Transform _enemyTransfrom) => targets.Add(_enemyTransfrom);

}
