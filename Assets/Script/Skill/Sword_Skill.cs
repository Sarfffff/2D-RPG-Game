using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SwordType
{
    Regular,//����
    Bounce,//����
    Pierce,//����
    Spin//��ת
}
public class Sword_Skill : Skill
{

    public SwordType swordType = SwordType.Regular;//Ĭ������ͨ

    [Header("����")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;//������ť
    [SerializeField] private int bounceAmount;//��������
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("����")]

    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;//������ť
    [SerializeField] private int pierceAmount;//���̴���
    [SerializeField] private float pierceGravity;

    [Header("��ת")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;//������ť
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;//������7
    [SerializeField] private float spinDuration = 2;  //ʱ��2
    [SerializeField] private float spinGravity;

    [Header("����")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;//������ť
    public bool swordUnlocked { get; private set; }//����
    [SerializeField] private GameObject swordPrefab;//����Ԥ����
    [SerializeField] private Vector2 launchForce;//���佣ʱӦ�õ����ȣ���һ����ά����
    [SerializeField] private float swordGravity;//�����������
    [SerializeField] private float freezeTimerDuration;
    [SerializeField] private float returnSpeed;
    private Vector2 finalDir;//�ӳ�ȥ�ķ���
    [Header("Aim dot")]
    [SerializeField] private int numberOfDots;  //������׼�ĸ����������
    [SerializeField] private float spaceBetweenDots;  // ÿ��������֮��ļ������
    [SerializeField] private GameObject dotPrefab;  // �������Ԥ����
    [SerializeField] private Transform dotsParent;  //���и�����ĸ�����������֯�����еĲ�νṹ
    private GameObject[] dots;//�洢�������ɵĸ�������������


    [Header("��������")]//��������
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;//ʱ��ֹͣ������ť
    public bool timeStopUnlocked;
    [SerializeField] private UI_SkillTreeSlot volnurableUnlockButton;//���˽�����ť
    public bool volnurableUnlocked;


    protected override void Start()
    {
        base.Start();
        GeneratorDots();
        SetupGravity();  //��ʼ����Ʋ�ͬ��������


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

    #region Unlock region//��������

    private void UnlockTimeStop()//����ʱ��ֹͣ
    {
        if (timeStopUnlockButton.unLocked&&!timeStopUnlocked)
            timeStopUnlocked = true;
    }

    private void UnlockVolnurable()//��������
    {
        if (volnurableUnlockButton.unLocked&&!volnurableUnlocked)
            volnurableUnlocked = true;
    }

    private void UnlockSword()//����Ͷ��������Ĭ��ѡ�� 
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

    protected override void Update()//���������ڼ����갴���ͷ��¼������¸������λ�ã������㽣�ķ��䷽��
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);//��λ����
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
        }
    }
    public void CreatSword()//��������ʵ�����������䷢�����Ⱥ�������ͬʱ������������и����㡣
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        //���д���ʹ�� Instantiate ����������һ�� swordPrefab��Ԥ��Ľ����󣩵���ʵ���������丳ֵ�� newSword ������
        //Instantiate ������������������Ҫʵ������Ԥ�����swordPrefab����ʵ������λ�ã�����ʹ�� player.transform.position������ҵĵ�ǰλ�ã����Լ�ʵ��������ת������ʹ�� transform.rotation������ǰ�������ת����
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();
        //���д���Ӹոմ����� newSword �����л�ȡ Sword_Skill_Controller �ű�������������丳ֵ�� newSwordScript ������Sword_Skill_Controller �ܿ�����һ���Զ���ű������ڿ��ƽ��ļ��ܺ���Ϊ��

        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);

        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimerDuration, returnSpeed);

        player.AssignNewSword(newSword);
        //���д�������� newSwordScript �� SetupSword ������������������������launchDir �� swordGravity�������������ܿ����������ý��ķ��䷽�򣨻��ƶ����򣩺�����Ӱ�졣
        DotsActive(false);

    }

#region AimZone
    public Vector2 AimDirection()//��������λ�õ����λ�õķ�������
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);//��ȡ���ĵ������
        Vector2 direction = mousePositon - playerPosition;
        return direction;
    }
    public void DotsActive(bool _isActive)//�������и�����ļ���״̬
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void GeneratorDots()//����ָ���������ͼ�����ɸ����㣬������������Ϊ������״̬��
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }
    private Vector2 DotsPosition(float t)//����ʱ�� t ���㲢���ظ������λ�á����λ�ÿ����˷��䷽�����Ⱥ��������������˶��Ĺ�ʽ
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
#endregion
}
