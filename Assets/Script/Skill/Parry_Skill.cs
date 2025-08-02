using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill  //����
{
    [Header("����")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("�����ָ�")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockedButton;
    public bool restoreUnlocked { get; private set; }
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;//���ƵĻָ���
    
    [Header("�������񹥻�")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlock {  get; private set; }


    public override void UseSkill()
    {
        base.UseSkill();
        if (restoreUnlocked)
        {
            int restoreAmount =Mathf.RoundToInt( player.stats.GetMaxHealthValue() * restoreHealthPercentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }


    protected override void Start()
    {
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParryReStore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPArryWithMirage);
    }
    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryReStore();
        UnlockPArryWithMirage();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unLocked&&!parryUnlocked)  //���ý�������
            parryUnlocked = true;
    }
    private void UnlockParryReStore()
    {
        if (restoreUnlockedButton.unLocked&&!restoreUnlocked)
            restoreUnlocked = true;
    }
    private void UnlockPArryWithMirage()
    {
        if (parryWithMirageUnlockButton.unLocked&&!parryWithMirageUnlock)
            parryWithMirageUnlock = true;
    }
    public void MakeMirageOnParry(Transform _respawnTransfrom)
    {
        if (parryWithMirageUnlock)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransfrom);
    }
}

