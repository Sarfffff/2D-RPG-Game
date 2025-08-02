using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill
{

    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton;
    public bool blackHoleUnlocked { get; private set; }


    [SerializeField] private float maxSize;//最大尺寸
    [SerializeField] private float growSpeed;//变大速度
    [SerializeField] private float shrinkSpeed;//缩小速度

    [SerializeField] private GameObject blackholePrefab;
    [Space]

    [SerializeField] private float blackholeDuration;
    [SerializeField] int amountOfAttacks = 4;
    [SerializeField] float cloneAttackCooldown = .3f;

    BlackHole_Skill_Controller currentBlackhole;
    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        UnlockBlackHole();
    }
    private void UnlockBlackHole()
    {
        if (blackHoleUnlockButton.unLocked&&!blackHoleUnlocked)
            blackHoleUnlocked = true;
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<BlackHole_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackholeDuration);

        AudioManager.instance.PlaySFX(3,player.transform);
        AudioManager.instance.PlaySFX(6,player.transform);
    }

    protected override void Start()
    {
        base.Start();
        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);

    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if (currentBlackhole == null)
            return false;
        if (currentBlackhole.playerCanExitState)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}