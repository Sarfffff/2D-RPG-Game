using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SwordType
{
    Regular,//常规
    Bounce,//反弹
    Pierce,//穿刺
    Spin//旋转
}
public class Sword_Skill : Skill
{

    public SwordType swordType = SwordType.Regular;//默认是普通

    [Header("弹跳")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;//解锁按钮
    [SerializeField] private int bounceAmount;//反弹次数
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("穿刺")]

    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;//解锁按钮
    [SerializeField] private int pierceAmount;//穿刺次数
    [SerializeField] private float pierceGravity;

    [Header("旋转")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;//解锁按钮
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;//最大距离7
    [SerializeField] private float spinDuration = 2;  //时间2
    [SerializeField] private float spinGravity;

    [Header("技能")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;//解锁按钮
    public bool swordUnlocked { get; private set; }//解锁
    [SerializeField] private GameObject swordPrefab;//剑的预制体
    [SerializeField] private Vector2 launchForce;//发射剑时应用的力度，是一个二维向量
    [SerializeField] private float swordGravity;//发射体的重力
    [SerializeField] private float freezeTimerDuration;
    [SerializeField] private float returnSpeed;
    private Vector2 finalDir;//扔出去的方向
    [Header("Aim dot")]
    [SerializeField] private int numberOfDots;  //用于瞄准的辅助点的数量
    [SerializeField] private float spaceBetweenDots;  // 每个辅助点之间的间隔距离
    [SerializeField] private GameObject dotPrefab;  // 辅助点的预制体
    [SerializeField] private Transform dotsParent;  //所有辅助点的父对象，用于组织场景中的层次结构
    private GameObject[] dots;//存储所有生成的辅助点对象的数组


    [Header("被动技能")]//被动技能
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;//时间停止解锁按钮
    public bool timeStopUnlocked;
    [SerializeField] private UI_SkillTreeSlot volnurableUnlockButton;//易伤解锁按钮
    public bool volnurableUnlocked;


    protected override void Start()
    {
        base.Start();
        GeneratorDots();
        SetupGravity();  //开始则控制不同剑的重力


        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);

        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);

        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        volnurableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVolnurable);
    
    }

    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockBounceSword();
        UnlockPierceSword();
        UnlockSpinSword();
        UnlockTimeStop();
        UnlockVolnurable();
    }

    #region Unlock region//解锁区域

    private void UnlockTimeStop()//解锁时间停止
    {
        if (timeStopUnlockButton.unLocked&&!timeStopUnlocked)
            timeStopUnlocked = true;
    }

    private void UnlockVolnurable()//解锁易伤
    {
        if (volnurableUnlockButton.unLocked&&!volnurableUnlocked)
            volnurableUnlocked = true;
    }

    private void UnlockSword()//解锁投掷武器。默认选择 
    {
        if (swordUnlockButton.unLocked&&!swordUnlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }

    private void UnlockBounceSword()
    {
        if (bounceUnlockButton.unLocked)
            swordType = SwordType.Bounce;
    }


    private void UnlockPierceSword()
    {
        if (pierceUnlockButton.unLocked)
            swordType = SwordType.Pierce;
    }

    private void UnlockSpinSword()
    {
        if (spinUnlockButton.unLocked)
            swordType = SwordType.Spin;
    }
    #endregion


    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;

    }

    protected override void Update()//这里它用于检测鼠标按键释放事件，更新辅助点的位置，并计算剑的发射方向
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);//单位向量
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
        }
    }
    public void CreatSword()//创建剑的实例，并设置其发射力度和重力。同时，它会禁用所有辅助点。
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        //这行代码使用 Instantiate 方法创建了一个 swordPrefab（预设的剑对象）的新实例，并将其赋值给 newSword 变量。
        //Instantiate 方法接受三个参数：要实例化的预设对象（swordPrefab），实例化的位置（这里使用 player.transform.position，即玩家的当前位置），以及实例化的旋转（这里使用 transform.rotation，即当前对象的旋转）。
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();
        //这行代码从刚刚创建的 newSword 对象中获取 Sword_Skill_Controller 脚本的组件，并将其赋值给 newSwordScript 变量。Sword_Skill_Controller 很可能是一个自定义脚本，用于控制剑的技能和行为。

        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);

        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimerDuration, returnSpeed);

        player.AssignNewSword(newSword);
        //这行代码调用了 newSwordScript 的 SetupSword 方法，并传递了两个参数：launchDir 和 swordGravity。这两个参数很可能用于配置剑的发射方向（或移动方向）和重力影响。
        DotsActive(false);

    }

#region AimZone
    public Vector2 AimDirection()//计算从玩家位置到鼠标位置的方向向量
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);//获取鼠标的点击方向
        Vector2 direction = mousePositon - playerPosition;
        return direction;
    }
    public void DotsActive(bool _isActive)//设置所有辅助点的激活状态
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void GeneratorDots()//根据指定的数量和间隔生成辅助点，并将它们设置为不激活状态。
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }
    private Vector2 DotsPosition(float t)//根据时间 t 计算并返回辅助点的位置。这个位置考虑了发射方向、力度和重力。抛物线运动的公式
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
#endregion
}
