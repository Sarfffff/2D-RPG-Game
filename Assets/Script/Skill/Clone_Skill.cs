using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("����")]
    [SerializeField] private float attackMutiplier;//��������
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;//clone����ʱ��
    [Space]


    [Header("������")]//��¡���������
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;  //���Ʒ����Ƿ��ܹ�����


    [Header("�����Է���")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }//�����Է���


    [Header("���ط���")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;//���ط���
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDublicateClone;//���ƾ���(�����������)
    [SerializeField] private float chanceToDuplicate;


    [Header("ˮ���滻����")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;//��¡�����ˮ����dash��counterAttack�ȵ�,���ǲ������clone���ṩѡ��clone����crystal


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
            attackMutiplier = cloneAttackMultiplier;//����Ĺ���������ֵ��attackMutiplier
        }
    }

    private void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unLocked && !canApplyOnHitEffect)
        {
            canApplyOnHitEffect = true;
            attackMutiplier = aggresiveCloneAttackMultiplier;//�����Է���Ĺ���������ֵ��attackMutiplier
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
 
    public void CreateClone(Transform _clonePosition, Vector3 _offset)//����clonePosition��ʾ��¡��Ӧ�ñ����õ�λ��
    {                                                               //����offset��ʾ����� _clonePosition ��ƫ������
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().
            SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindCloseEnemy(newClone.transform), canDublicateClone, chanceToDuplicate, player,attackMutiplier);
    }



    #region ���е������ӳ����ɾ�����й���
    public void CreateCloneWithDelay(Transform _enemyTransform)//����clone���е�������
    {
         StartCoroutine(CloneDelayCorotine(_enemyTransform, new Vector3(1 * player.facingDir, 0)));
    }
    private IEnumerator CloneDelayCorotine(Transform _transform, Vector3 _offset)  //Э���ӳ����ɾ���
    {

        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
    #endregion
}
