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

    [Header("ˮ��������(Ĭ��)")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked {  get; private set; }

    [Header("ˮ��������λ��")]
    [SerializeField] private bool cloneInsteadOfCrystal;  //����ˮ�����ٴΰ��¸�ˮ����λ��
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadCrtstalButton;


    [Header("��ըˮ��")]
    [SerializeField] private UI_SkillTreeSlot unlockExplodeButton;
    [SerializeField] private bool canExplode;
    [SerializeField] private float explodeCooldown;

    [Header("ˮ���ƶ�")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("���ѵ�����")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
    [SerializeField] private bool canUseMultiStacks;//����Ƿ����ʹ�ö�ѵ�ˮ���Ĺ���
    [SerializeField] private int amountOfStacks;//ˮ������
    [SerializeField] private float multiStackCooldown;//���ܵ���ȴʱ��
    [SerializeField] private float useTimeWindow;

    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();//GameObject�б����ڴ洢ˮ��
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
    #region ������(���ܽ���)

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
        else//�����ǰ��ˮ��ʵ����canMoveToEnemyΪfalse���򽻻���Һ�ˮ����λ�ã����򣬲��ܽ���λ��
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
                currentCrystal.GetComponent<CryStalSkill_Controller>()?.FinishCrystal();//����ˮ���ϵ�FinishCrystal����������ˮ���ĵ�ǰ״̬����Ϊ��
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
    // �ж��Ƿ��ܹ�ʹ�ö�ˮ��,�����Ƿ�ʹ���꼼�ܣ���������ȴ 
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks) // ����Ƿ�����ʹ�ö�ѵ�ˮ��
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
    private void ResetAbility()  //ÿ����ȴ��������д���ü���
    {
        if (cooldownTimer > 0)
        {
            return;
        }
        cooldown = multiStackCooldown;
        RefiCrystal();
    }
}
