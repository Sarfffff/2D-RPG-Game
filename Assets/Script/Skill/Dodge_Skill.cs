using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill //躲避
{
    [Header("闪避")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int eavsionAmount; //学习技能后闪避增加10，默认技能
    public bool dodgeunlocked;

    [Header("闪避镜像")]
    [SerializeField] private UI_SkillTreeSlot unlockMIrageDodgeButton;
    public bool dodgeMirageunlocked;
    protected override void Start()
    {
        base.Start();
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMIrageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }
    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }
    private void UnlockDodge()
    {

        if (unlockDodgeButton.unLocked&&!dodgeunlocked)
        {
            player.stats.evasion.AddModifers(eavsionAmount);
            Inventory.Instance.UpdateStatsUI();  //增加闪避点后进行更新
            dodgeunlocked = true;
        }
      
    }    
    private void UnlockMirageDodge()
    {
        if(unlockMIrageDodgeButton.unLocked&&!dodgeMirageunlocked)
            dodgeMirageunlocked = true;
    }
    public  void CreateMirageOnDodge()
    {
        if (dodgeMirageunlocked)
            SkillManager.instance.clone.CreateClone(player.transform,new Vector2(2 *player.facingDir,0));
    }
}
