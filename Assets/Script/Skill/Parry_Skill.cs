using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill  //弹反
{
    [Header("弹反")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("弹反恢复")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockedButton;
    public bool restoreUnlocked { get; private set; }
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;//治疗的恢复量
    
    [Header("弹反镜像攻击")]
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
        if (parryUnlockButton.unLocked&&!parryUnlocked)  //设置解锁条件
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

