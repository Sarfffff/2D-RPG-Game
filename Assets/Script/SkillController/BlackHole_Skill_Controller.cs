using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;//最大尺寸
    private float growSpeed;//变大速度
    private float shrinkSpeed;//缩小速度
    private float blackholeTimer;

    private bool canGrow = true;//是否可以变大
    private bool canShrink;//缩小
    private bool canCreateHotKeys = true; //专门控制后面进入的没法生成热键
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
        if(blackholeTimer <= 0 ) //黑洞持续时间结束
        {
            blackholeTimer = Mathf.Infinity;  //将时间设置为无限长
            if(targets.Count > 0)   //如果敌人数量>0，释放攻击，没有则开始进入退出的过程
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
            //这是控制物体大小的参数
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            //类似MoveToward，不过是放大到多少大小 \
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
            if (Random.Range(0, 100) > 50)  //x轴产生偏移量
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
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xoffSet, 0)); //创建的克隆体会产生偏移量

            }
            amountOfAttacks--;
            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleAbility", 1f);
            }
        }
    }

    private void FinishBlackHoleAbility()    //进行退出动画，完成黑洞攻击
    {
        DestroyHotKey();
        playerCanExitState = true;  //退出
        canShrink = true;  //缩小
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
         //创建实例
        GameObject newHotkey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        //将实例添加进列表
        createHotKey.Add(newHotkey);
        //随机KeyCode传给HotKey，并且传过去一个毁掉一个
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];

        keyCodeList.Remove(choosenKey);

        BlackHotKey_Controller newHotkeyScript = newHotkey.GetComponent<BlackHotKey_Controller>();
        newHotkeyScript.SetupHotKey(choosenKey, collision.transform, this);
        //newHotkey.GetComponent<BlackHole_Skill_Controller>.SetupHotKey();
        // targets.Add(collision.transform);
    }
    public void AddEnemyToList(Transform _enemyTransfrom) => targets.Add(_enemyTransfrom);

}
