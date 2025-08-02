using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("���")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockedButton;
    public bool dashUnlocked { get; private set; }

    [Header("��̿�ʼ��������")]
    [SerializeField] private UI_SkillTreeSlot cloneOndashUnlockedButton;
    public bool cloneOndashUnlocked { get; private set; }

    [Header("��̽�����������")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockedButton;
    public bool cloneOnArrivalUnlocked { get; private set; }




    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();
        dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOndashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneArrival);
    }
    override protected void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneArrival();
    }

    private void UnlockDash()
    {
        if(dashUnlockedButton.unLocked &&!dashUnlocked)  //���ý�������
            dashUnlocked = true;
    }
    private void UnlockCloneOnDash()
    {
        if(cloneOndashUnlockedButton.unLocked && !cloneOndashUnlocked)
            cloneOndashUnlocked = true;
    }
    private void UnlockCloneArrival() {
        if(cloneOnArrivalUnlockedButton.unLocked && !cloneOnArrivalUnlocked)
            cloneOnArrivalUnlocked = true;
    }

    public void CloneOnDash()
    {
        if (cloneOndashUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }  //���ǰ����clone
    public void CloneOnDashArrival()
    {
        if (cloneOnArrivalUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }   //��̺�����clone

}
