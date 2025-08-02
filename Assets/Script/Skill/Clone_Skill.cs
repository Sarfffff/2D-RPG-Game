using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("分身")]
    [SerializeField] private float attackMutiplier;//攻击乘数
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;//clone持续时间
    [Space]


    [Header("分身攻击")]//克隆体产生镜像
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;  //控制分身是否能够攻击


    [Header("侵略性分身")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }//侵略性分身


    [Header("多重分身")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;//多重分身
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDublicateClone;//复制镜像(镜像产生镜像)
    [SerializeField] private float chanceToDuplicate;


    [Header("水晶替换分身")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;//克隆体产生水晶，dash，counterAttack等等,但是不会产生clone，提供选择clone还是crystal


    #region Unlock region
    protected override void Start()
    {
        base.Start();


        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCryStalInstead);


    }
    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggressiveClone();
        UnlockMultipleClone();
        UnlockCryStalInstead();
    }
    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unLocked&&!canAttack)
        {
            canAttack = true;
            attackMutiplier = cloneAttackMultiplier;//幻象的攻击次数赋值给attackMutiplier
        }
    }

    private void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unLocked && !canApplyOnHitEffect)
        {
            canApplyOnHitEffect = true;
            attackMutiplier = aggresiveCloneAttackMultiplier;//侵略性分身的攻击次数赋值给attackMutiplier
        }
    }

    private void UnlockMultipleClone()
    {
        if (multipleUnlockButton.unLocked && !canDublicateClone)
        {
            canDublicateClone = true;
            attackMutiplier = multiCloneAttackMultiplier;
        }
    }

    private void UnlockCryStalInstead()
    {
        if (crystalInsteadUnlockButton.unLocked && !crystalInsteadOfClone)
        {
            crystalInsteadOfClone = true;
        }
    }
 
    #endregion
 
    public void CreateClone(Transform _clonePosition, Vector3 _offset)//——clonePosition表示克隆体应该被放置的位置
    {                                                               //——offset表示相对于 _clonePosition 的偏移量。
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().
            SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindCloseEnemy(newClone.transform), canDublicateClone, chanceToDuplicate, player,attackMutiplier);
    }



    #region 进行弹反后延迟生成镜像进行攻击
    public void CreateCloneWithDelay(Transform _enemyTransform)//创建clone进行弹反攻击
    {
         StartCoroutine(CloneDelayCorotine(_enemyTransform, new Vector3(1 * player.facingDir, 0)));
    }
    private IEnumerator CloneDelayCorotine(Transform _transform, Vector3 _offset)  //协程延迟生成镜像
    {

        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
    #endregion
}
