using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill //���
{
    [Header("����")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int eavsionAmount; //ѧϰ���ܺ���������10��Ĭ�ϼ���
    public bool dodgeunlocked;

    [Header("���ܾ���")]
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
            Inventory.Instance.UpdateStatsUI();  //�������ܵ����и���
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
