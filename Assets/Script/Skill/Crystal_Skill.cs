using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("水晶技能树(默认)")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked {  get; private set; }

    [Header("水晶替身（换位）")]
    [SerializeField] private bool cloneInsteadOfCrystal;  //生成水晶，再次按下跟水晶换位置
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadCrtstalButton;


    [Header("爆炸水晶")]
    [SerializeField] private UI_SkillTreeSlot unlockExplodeButton;
    [SerializeField] private bool canExplode;
    [SerializeField] private float explodeCooldown;

    [Header("水晶移动")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("多层堆叠晶体")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
    [SerializeField] private bool canUseMultiStacks;//玩家是否可以使用多堆叠水晶的功能
    [SerializeField] private int amountOfStacks;//水晶数量
    [SerializeField] private float multiStackCooldown;//技能的冷却时间
    [SerializeField] private float useTimeWindow;

    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();//GameObject列表，用于存储水晶
    protected override void Start()
    {
        base.Start();
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(unlockCrystal);
        unlockCloneInsteadCrtstalButton.GetComponent<Button>().onClick.AddListener(unlockCrystalMirage);
        unlockExplodeButton.GetComponent<Button>().onClick.AddListener(unlockExplodsiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(unlockMovingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(unlockMultiStack);
    }
    protected override void CheckUnlock()
    {
        unlockCrystal();
        unlockCrystalMirage();
        unlockExplodsiveCrystal();
        unlockMovingCrystal();
        unlockMultiStack();
    }
    #region 技能树(技能解锁)

    private void unlockCrystal()
    {
        if(unlockCrystalButton.unLocked)
            crystalUnlocked = true;
    }
    private void unlockCrystalMirage()
    {
        if (unlockCloneInsteadCrtstalButton.unLocked&&!cloneInsteadOfCrystal)
            cloneInsteadOfCrystal = true; 
    }
    private void unlockExplodsiveCrystal()
    {
        if (unlockExplodeButton.unLocked && !canExplode)
        {

            canExplode = true;
            cooldown = explodeCooldown;
        }
    }
    private void unlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unLocked && !canMoveToEnemy)
            canMoveToEnemy = true;
    }
    private void unlockMultiStack()
    {
        if (unlockMultiStackButton.unLocked && !canUseMultiStacks)
            canUseMultiStacks = true;
    }

    #endregion
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else//如果当前有水晶实例且canMoveToEnemy为false，则交换玩家和水晶的位置，否则，不能交换位置
        {
            if (canMoveToEnemy)
                return;
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;


            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<CryStalSkill_Controller>()?.FinishCrystal();//调用水晶上的FinishCrystal方法来结束水晶的当前状态或行为。
            }


        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CryStalSkill_Controller currentCrystalScript = currentCrystal.GetComponent<CryStalSkill_Controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindCloseEnemy(currentCrystal.transform), player);

    }
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<CryStalSkill_Controller>().ChooseRandomEnemy();
    // 判断是否能够使用多水晶,无论是否使用完技能，都进入冷却 
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks) // 检查是否允许使用多堆叠水晶
        {
            if (crystalLeft.Count > 0 && cooldownTimer < 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }
                    cooldown = 0;
                    GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                    if (crystalToSpawn != null && player != null)
                    {
                        GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                        crystalLeft.Remove(crystalToSpawn);
                        if (newCrystal != null)
                        {
                            Transform closeEnemy = FindCloseEnemy(crystalToSpawn.transform);
                            if (closeEnemy != null)
                            {
                                newCrystal.GetComponent<CryStalSkill_Controller>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, closeEnemy, player);
                            }
                        }
                    }
                    if (crystalLeft.Count <= 0)
                    {
                        cooldown = multiStackCooldown;
                        RefiCrystal();
                    }
                    return true;
            }
            
        }
        return false;
    }

    private void RefiCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }
    private void ResetAbility()  //每次冷却结束，重写设置技能
    {
        if (cooldownTimer > 0)
        {
            return;
        }
        cooldown = multiStackCooldown;
        RefiCrystal();
    }
}
